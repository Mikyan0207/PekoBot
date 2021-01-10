using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NLog;
using NLog.Fluent;
using PekoBot.Core.Modules.VTubers.Common;
using PekoBot.Core.Services;
using PekoBot.Core.Services.Impl;
using PekoBot.Entities;
using PekoBot.Entities.Models;
using DiscordChannel = PekoBot.Entities.Models.DiscordChannel;

namespace PekoBot.Core.Modules.VTubers.Services
{
	public class HololiveService : IService
	{
		private static readonly string API_URL = "https://holo.dev/api/v1/";
		private static readonly string CURRENT_LIVE_ENDPOINT = "lives/current";
		private static readonly string SCHEDULED_LIVE_ENDPOINT = "lives/scheduled";
		private static readonly string ENDED_LIVE_ENDPOINT = "lives/ended";

		private static HttpClient HttpClient { get; set; }

		private DbService DbService { get; }

		private DiscordClient Client { get; }

		public HololiveService(DbService dbService, DiscordClient client)
		{
			DbService = dbService;
			Client = client;

			HttpClient = new HttpClient(new HttpClientHandler()
			{
				AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
			}, true);

			HttpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.163 Safari/537.36");
		}

		public async Task<string> GetAsync(string url)
		{
			return await HttpClient.GetStringAsync(new Uri(url)).ConfigureAwait(false);
		}

		private async Task<Member> GetMemberAsync(string channelId)
		{
			return await DbService
				.GetContext()
				.Members
				.FirstOrDefaultAsync(x => x.YoutubeId == channelId)
				.ConfigureAwait(false);
		}

		private async Task<Live> GetLiveAsync(int liveId)
		{
			return await DbService
				.GetContext()
				.Lives
				.FirstOrDefaultAsync(x => x.LiveId == liveId)
				.ConfigureAwait(false);
		}

		private async Task<List<DiscordChannel>> GetNotificationsChannelsAsync()
		{
			return await DbService
				.GetContext()
				.Channels
				.Where(x => x.ChannelType == Entities.Models.ChannelType.HololiveNotifications)
				.ToListAsync()
				.ConfigureAwait(false);
		}

		private async Task<Live> AddLiveAsync(HololiveLive live)
		{
			try
			{
				var e = await DbService.GetContext().AddAsync(new Live()
				{
					LiveId = live.Id,
					Title = live.Title,
					StartAt = live.StartAt,
					CreatedAt = live.CreatedAt,
					Cover = live.Cover,
					Room = live.Room,
					Platform = live.Platform == "youtube" ? Platform.Youtube : Platform.Other,
					Member = await GetMemberAsync(live.Channel).ConfigureAwait(false),
					Reminded = false
				}).ConfigureAwait(false);
				await DbService.GetContext().SaveChangesAsync().ConfigureAwait(false);

				LogManager.GetCurrentClassLogger().Info($"New live added. Id = {live.Id}");

				return e.Entity;
			}
			catch (DbUpdateConcurrencyException e)
			{
				LogManager.GetCurrentClassLogger().Error(e);
				throw;
			}
		}

		private async Task UpdateLiveAsync(Live live)
		{
			try
			{
				DbService.GetContext().Update(live);
				await DbService.GetContext().SaveChangesAsync().ConfigureAwait(false);
			}
			catch (DbUpdateConcurrencyException e)
			{
				LogManager.GetCurrentClassLogger().Error(e);
				throw;
			}
			DbService.GetContext().Update(live);
			await DbService.GetContext().SaveChangesAsync().ConfigureAwait(false);
		}

		private async Task RemoveLiveAsync(Live live)
		{
			try
			{
				DbService.GetContext().Remove(live);
				await DbService.GetContext().SaveChangesAsync().ConfigureAwait(false);
			}
			catch (DbUpdateConcurrencyException e)
			{
				LogManager.GetCurrentClassLogger().Error(e);
				throw;
			}
		}

		private async Task HololiveScheduledLivesHandler(List<DiscordChannel> channels)
		{
			var apiResponse = await GetAsync(API_URL + SCHEDULED_LIVE_ENDPOINT).ConfigureAwait(false);
			var lives = JsonConvert.DeserializeObject<HololiveLives>(apiResponse);

			foreach (var live in lives.Lives)
			{
				var cachedLive = await GetLiveAsync(live.Id).ConfigureAwait(false);

				if (cachedLive == null)
				{
					var member = await GetMemberAsync(live.Channel).ConfigureAwait(false);

					if (member == null)
						continue;

					if (live.StartAt - DateTime.Now > TimeSpan.FromHours(6))
						continue;

					if (live.StartAt - DateTime.Now < TimeSpan.FromSeconds(0))
						continue;

					var e = await AddLiveAsync(live).ConfigureAwait(false);

					foreach (var channel in channels)
					{
						await Client.SendMessageAsync(await Client.GetChannelAsync(channel.ChannelId),
							embed: CreateNotificationEmbed(e)).ConfigureAwait(false);

						Thread.Sleep(TimeSpan.FromSeconds(5));
					}
				}
				else if (cachedLive.StartAt - DateTime.Now <= TimeSpan.FromMinutes(30) && cachedLive.StartAt - DateTime.Now > TimeSpan.FromSeconds(0) && !cachedLive.Reminded)
				{
					var member = await GetMemberAsync(live.Channel).ConfigureAwait(false);

					if (member == null)
						continue;

					foreach (var channel in channels)
					{
						await Client.SendMessageAsync(await Client.GetChannelAsync(channel.ChannelId),
							embed: CreateReminderEmbed(cachedLive)).ConfigureAwait(false);

						Thread.Sleep(TimeSpan.FromSeconds(5));

						cachedLive.Reminded = true;
						await UpdateLiveAsync(cachedLive).ConfigureAwait(false);
					}
				}
			}

		}

		private async Task HololiveCurrentLivesHandler(List<DiscordChannel> channels)
		{
			var lives = await DbService.GetContext().Lives.ToListAsync().ConfigureAwait(false);

			foreach (var live in lives)
			{
				if (live.StartAt - DateTime.Now <= TimeSpan.FromMinutes(1) ||
				    live.StartAt - DateTime.Now >= TimeSpan.FromMinutes(-5))
				{
					foreach (var channel in channels)
					{
						await Client.SendMessageAsync(await Client.GetChannelAsync(channel.ChannelId),
							embed: CreateReminderEmbed(live)).ConfigureAwait(false);

						Thread.Sleep(TimeSpan.FromSeconds(5));
						await RemoveLiveAsync(live);
					}
				}
			}
		}

		public async Task HololiveNotificationsHandler()
		{
			var channels = await GetNotificationsChannelsAsync().ConfigureAwait(false);

			while (true)
			{
				await HololiveScheduledLivesHandler(channels).ConfigureAwait(false);
				Thread.Sleep(TimeSpan.FromSeconds(5));
				await HololiveCurrentLivesHandler(channels).ConfigureAwait(false);

				Thread.Sleep(TimeSpan.FromMinutes(30));
			}
		}

		public DiscordEmbed CreateNotificationEmbed(Live live)
		{
			var t = (live.StartAt - DateTime.Now);
			var embed = new DiscordEmbedBuilder()
				.WithColor(DiscordColor.Red)
				.WithImageUrl(live.Cover)
				.WithTitle($"{live.Member.Name} scheduled a new live")
				.WithDescription(live.Title)
				.WithUrl($"https://www.youtube.com/watch?v={live.Room}")
				.WithAuthor(live.Member.YoutubeName, live.Member.YoutubeUrl, live.Member.YoutubeAvatarUrl)
				.AddField("Platform", live.Platform == Platform.Youtube ? "YouTube" : "Other", true)
				.AddField("Start in", t.ToString("c"), true);

			return embed.Build();
		}

		public DiscordEmbed LiveNotificationEmbed(Live live)
		{
			var t = (live.StartAt - DateTime.Now);
			var embed = new DiscordEmbedBuilder()
				.WithColor(DiscordColor.Red)
				.WithImageUrl(live.Cover)
				.WithTitle($"{live.Member.Name} is live now!")
				.WithDescription(live.Title)
				.WithUrl($"https://www.youtube.com/watch?v={live.Room}")
				.WithAuthor(live.Member.YoutubeName, live.Member.YoutubeUrl, live.Member.YoutubeAvatarUrl)
				.AddField("Platform", live.Platform == Platform.Youtube ? "YouTube" : "Other", true);

			return embed.Build();
		}

		public DiscordEmbed CreateReminderEmbed(Live live)
		{
			var t = (live.StartAt - DateTime.Now);
			var embed = new DiscordEmbedBuilder()
				.WithColor(DiscordColor.Red)
				.WithImageUrl(live.Cover)
				.WithTitle($"{live.Member.Name}'s live will be starting soon")
				.WithDescription(live.Title)
				.WithUrl($"https://www.youtube.com/watch?v={live.Room}")
				.WithAuthor(live.Member.YoutubeName, live.Member.YoutubeUrl, live.Member.YoutubeAvatarUrl)
				.AddField("Platform", live.Platform == Platform.Youtube ? "YouTube" : "Other", true)
				.AddField("Start in", t.ToString("c"), true);

			return embed.Build();
		}
	}
}
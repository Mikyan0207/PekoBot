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
using PekoBot.Database;
using PekoBot.Entities;
using PekoBot.Entities.Models;
using ChannelType = PekoBot.Entities.Models.ChannelType;
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

		private static Logger Logger { get; set; }

		private DbService DbService { get; }
		private PekoBotContext Context { get; }

		private DiscordClient Client { get; }

		public HololiveService(DbService dbService, DiscordClient client)
		{
			Logger = LogManager.GetCurrentClassLogger();
			DbService = dbService;
			Context = DbService.GetContext();
			Client = client;

			HttpClient = new HttpClient(new HttpClientHandler()
			{
				AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
			}, true);

			HttpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.163 Safari/537.36");
		}

		public Task RunHandlersAsync()
		{
			var t1 = Task.Run(async () =>
			{
				await HololiveScheduledLivesHandler().ConfigureAwait(false);
			});

			var t2 = Task.Run(async () =>
			{
				await HololiveReminderHandler().ConfigureAwait(false);
			});

			var t3 = Task.Run(async () =>
			{
				await HololiveCurrentLivesHandler().ConfigureAwait(false);
			});

			return Task.CompletedTask;
		}

		private static async Task<string> GetAsync(string url)
		{
			return await HttpClient.GetStringAsync(new Uri(url)).ConfigureAwait(false);
		}

		private async Task<Member> GetMemberAsync(string channelId)
		{
			return await Context
				.Members
				.FirstOrDefaultAsync(x => x.YoutubeId == channelId)
				.ConfigureAwait(false);
		}

		private async Task<Live> GetLiveAsync(int liveId)
		{
			return await Context
				.Lives
				.FirstOrDefaultAsync(x => x.LiveId == liveId)
				.ConfigureAwait(false);
		}

		private async Task<List<DiscordChannel>> GetNotificationsChannelsAsync()
		{
			return await Context
				.Channels
				.Where(x => x.ChannelType == Entities.Models.ChannelType.HololiveNotifications)
				.ToListAsync()
				.ConfigureAwait(false);
		}

		private async Task<Live> AddLiveAsync(HololiveLive live)
		{
			try
			{
				var e = await Context.AddAsync(new Live()
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
				await Context.SaveChangesAsync().ConfigureAwait(false);

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
				Context.Update(live);
				await Context.SaveChangesAsync().ConfigureAwait(false);
			}
			catch (DbUpdateConcurrencyException e)
			{
				LogManager.GetCurrentClassLogger().Error(e);
				throw;
			}
		}

		private async Task RemoveLiveAsync(Live live)
		{
			try
			{
				Context.Remove(live);
				await Context.SaveChangesAsync().ConfigureAwait(false);
			}
			catch (DbUpdateConcurrencyException e)
			{
				LogManager.GetCurrentClassLogger().Error(e);
				throw;
			}
		}

		private async Task HololiveScheduledLivesHandler()
		{
			while (true)
			{
				Logger.Info("Checking scheduled lives...");

				var apiResponse = await GetAsync(API_URL + SCHEDULED_LIVE_ENDPOINT).ConfigureAwait(false);
				var lives = JsonConvert.DeserializeObject<HololiveLives>(apiResponse);
				var channels = await Context.Channels.Where(x => x.ChannelType == ChannelType.HololiveNotifications)
					.ToListAsync().ConfigureAwait(false);

				foreach (var live in lives.Lives)
				{
					var cachedLive = await GetLiveAsync(live.Id).ConfigureAwait(false);

					if (cachedLive != null)
						continue;

					var member = await GetMemberAsync(live.Channel).ConfigureAwait(false);

					if (member == null)
						continue;
					if ((live.StartAt - DateTime.Now).Duration() >= TimeSpan.FromHours(6))
						continue;

					var e = await AddLiveAsync(live).ConfigureAwait(false);

					foreach (var channel in channels)
					{
						await Client.SendMessageAsync(await Client.GetChannelAsync(channel.ChannelId),
							embed: CreateNotificationEmbed(e)).ConfigureAwait(false);

						Thread.Sleep(TimeSpan.FromSeconds(5));
					}
				}

				await Task.Delay(TimeSpan.FromMinutes(30));
			}
		}

		private async Task HololiveReminderHandler()
		{
			while (true)
			{
				Logger.Info("Checking reminder...");

				var channels = await Context
					.Channels
					.Where(x => x.ChannelType == ChannelType.HololiveNotifications)
					.ToListAsync()
					.ConfigureAwait(false);

				var lives = await Context
					.Lives
					.Where(x => x.Reminded == false)
					.ToListAsync().ConfigureAwait(false);

				foreach (var live in lives.Where(live => (live.StartAt - DateTime.Now).Duration() <= TimeSpan.FromMinutes(15)))
				{
					live.Reminded = true;
					await UpdateLiveAsync(live).ConfigureAwait(false);

					foreach (var channel in channels)
					{
						await Client.SendMessageAsync(await Client.GetChannelAsync(channel.ChannelId),
							embed: CreateReminderEmbed(live)).ConfigureAwait(false);

						Thread.Sleep(TimeSpan.FromSeconds(5));
					}
				}

				await Task.Delay(TimeSpan.FromMinutes(15));
			}
		}

		private async Task HololiveCurrentLivesHandler()
		{
			while (true)
			{
				Logger.Info("Checking current lives...");

				var channels = await Context
					.Channels
					.Where(x => x.ChannelType == ChannelType.HololiveNotifications)
					.ToListAsync()
					.ConfigureAwait(false);

				var lives = await Context
					.Lives
					.Where(x => x.Reminded && !x.Notified)
					.ToListAsync().ConfigureAwait(false);

				foreach (var live in lives.Where(live => (live.StartAt - DateTime.Now).Duration() <= TimeSpan.FromMinutes(5)))
				{
					live.Notified = true;
					await UpdateLiveAsync(live).ConfigureAwait(false);

					foreach (var channel in channels)
					{
						await Client.SendMessageAsync(await Client.GetChannelAsync(channel.ChannelId),
							embed: LiveNotificationEmbed(live)).ConfigureAwait(false);

						Thread.Sleep(TimeSpan.FromSeconds(5));
					}
				}

				await Task.Delay(TimeSpan.FromMinutes(5));
			}
		}

		private static DiscordEmbed CreateNotificationEmbed(Live live)
		{
			var t = (live.StartAt - DateTime.Now).Duration();
			var embed = new DiscordEmbedBuilder()
				.WithColor(DiscordColor.Red)
				.WithImageUrl(live.Cover)
				.WithTitle($"{live.Member.Name} scheduled a new live")
				.WithDescription(live.Title)
				.WithUrl($"https://www.youtube.com/watch?v={live.Room}")
				.WithAuthor(live.Member.YoutubeName, live.Member.YoutubeUrl, live.Member.YoutubeAvatarUrl)
				.WithFooter($"{live.StartAt.ToShortDateString()} {live.StartAt.ToShortTimeString()}", "https://png.pngtree.com/element_our/sm/20180301/sm_5a9797d5c93d3.jpg")
				.AddField("Platform", live.Platform == Platform.Youtube ? "YouTube" : "Other", true)
				.AddField("Start in", t.ToString(@"hh\:mm\:ss"), true);

			return embed.Build();
		}

		private static DiscordEmbed LiveNotificationEmbed(Live live)
		{
			var embed = new DiscordEmbedBuilder()
				.WithColor(DiscordColor.Red)
				.WithImageUrl(live.Cover)
				.WithTitle($"{live.Member.Name} is live now!")
				.WithDescription(live.Title)
				.WithUrl($"https://www.youtube.com/watch?v={live.Room}")
				.WithFooter($"{live.StartAt.ToShortDateString()} {live.StartAt.ToShortTimeString()}", "https://png.pngtree.com/element_our/sm/20180301/sm_5a9797d5c93d3.jpg")
				.WithAuthor(live.Member.YoutubeName, live.Member.YoutubeUrl, live.Member.YoutubeAvatarUrl)
				.AddField("Platform", live.Platform == Platform.Youtube ? "YouTube" : "Other", true);

			return embed.Build();
		}

		private static DiscordEmbed CreateReminderEmbed(Live live)
		{
			var t = (live.StartAt - DateTime.Now).Duration();
			var embed = new DiscordEmbedBuilder()
				.WithColor(DiscordColor.Red)
				.WithImageUrl(live.Cover)
				.WithTitle($"{live.Member.Name}'s live will be starting soon")
				.WithDescription(live.Title)
				.WithUrl($"https://www.youtube.com/watch?v={live.Room}")
				.WithAuthor(live.Member.YoutubeName, live.Member.YoutubeUrl, live.Member.YoutubeAvatarUrl)
				.WithFooter($"{live.StartAt.ToShortDateString()} {live.StartAt.ToShortTimeString()}", "https://png.pngtree.com/element_our/sm/20180301/sm_5a9797d5c93d3.jpg")
				.AddField("Platform", live.Platform == Platform.Youtube ? "YouTube" : "Other", true)
				.AddField("Start in", t.ToString(@"hh\:mm\:ss"), true);

			return embed.Build();
		}
	}
}
using System;
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
using PekoBot.Core.Modules.VTubers.Common;
using PekoBot.Core.Services;
using PekoBot.Core.Services.Impl;
using PekoBot.Entities;
using PekoBot.Entities.Models;
using ChannelType = PekoBot.Entities.Enums.ChannelType;

namespace PekoBot.Core.Modules.VTubers.Services
{
	public class HololiveService : IService
	{
		private static readonly string API_URL = "https://holo.dev/api/v1/";
		private static readonly string SCHEDULED_LIVE_ENDPOINT = "lives/scheduled";

		private static HttpClient HttpClient { get; set; }

		private static Logger Logger { get; set; }

		private DbService DbService { get; set; }

		private DiscordClient Client { get; }

		private CancellationTokenSource ScheduledLiveTokenSource { get; set; } 
		private CancellationToken ScheduledLiveToken { get; set; }

		private CancellationTokenSource CurrentLiveTokenSource { get; set; }
		private CancellationToken CurrentLiveToken { get; set; }

		public HololiveService(DbService dbService, DiscordClient client)
		{
			Logger = LogManager.GetCurrentClassLogger();
			DbService = dbService;
			Client = client;

			HttpClient = new HttpClient(new HttpClientHandler()
			{
				AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
			}, true);

			HttpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.163 Safari/537.36");
		}

		public Task RunHandlersAsync()
		{
			ScheduledLiveTokenSource = new CancellationTokenSource();
			ScheduledLiveToken = ScheduledLiveTokenSource.Token;

			CurrentLiveTokenSource = new CancellationTokenSource();
			CurrentLiveToken = CurrentLiveTokenSource.Token;

			_ = Task.Run(async () =>
			{
				try
				{
					await HololiveScheduledLivesHandler().ConfigureAwait(false);
				}
				catch (Exception e)
				{
					Logger.Error(e);
					throw;
				}
			}, ScheduledLiveTokenSource.Token);

			_ = Task.Run(async () =>
			{
				try
				{
					await HololiveCurrentLivesHandler().ConfigureAwait(false);
				}
				catch (Exception e)
				{
					Logger.Error(e);
					throw;
				}
			}, CurrentLiveTokenSource.Token);

			return Task.CompletedTask;
		}

		public bool StopHandlers()
		{
			if (ScheduledLiveTokenSource == null && CurrentLiveTokenSource == null)
				return false;

			ScheduledLiveTokenSource?.Cancel();
			CurrentLiveTokenSource?.Cancel();

			return true;
		}

		private static async Task<string> GetAsync(string url)
		{
			return await HttpClient.GetStringAsync(new Uri(url)).ConfigureAwait(false);
		}

		public async Task<Member> GetMemberAsync(string channelId)
		{
			using var uow = DbService.GetUnitOfWork();

			return await uow.Members.GetByChannelIdAsync(channelId).ConfigureAwait(false);
		}

		private async Task<Live> GetLiveAsync(int liveId)
		{
			using var uow = DbService.GetUnitOfWork();

			return await uow.Lives.GetLiveByIdAsync(liveId).ConfigureAwait(false);
		}

		private async Task<Live> AddLiveAsync(HololiveLive live)
		{
			try
			{
				using var uow = DbService.GetUnitOfWork();

				var e = new Live
				{
					LiveId = live.Id,
					Title = live.Title,
					StartAt = live.StartAt,
					CreatedAt = live.CreatedAt,
					Cover = live.Cover,
					Room = live.Room,
					Platform = live.Platform == "youtube" ? Platform.Youtube : Platform.Other,
					Member = await uow.Members.GetByChannelIdAsync(live.Channel).ConfigureAwait(false),
					Notified = false
				};

				await uow.Lives.AddAsync(e).ConfigureAwait(false);
				await uow.SaveChangesAsync().ConfigureAwait(false);

				return e;
			}
			catch (DbUpdateConcurrencyException e)
			{
				Logger.Error(e);
				throw;
			}
		}

		private async Task HololiveScheduledLivesHandler()
		{
			while (true)
			{
				if (ScheduledLiveToken.IsCancellationRequested)
					break;

				var apiResponse = await GetAsync(API_URL + SCHEDULED_LIVE_ENDPOINT).ConfigureAwait(false);
				var lives = JsonConvert.DeserializeObject<HololiveLives>(apiResponse);
				var channels = await DbService
					.GetContext()
					.Channels
					.Where(x => x.ChannelType == ChannelType.Hololive)
					.ToListAsync()
					.ConfigureAwait(false);

				Logger.Info($"Checking scheduled {lives.TotalLives} lives...");

				foreach (var live in lives.Lives)
				{
					var cachedLive = await GetLiveAsync(live.Id).ConfigureAwait(false);

					if (cachedLive != null)
						continue;

					var member = await GetMemberAsync(live.Channel).ConfigureAwait(false);

					if (member == null)
						continue;
					if ((live.StartAt - DateTime.UtcNow).Duration() >= TimeSpan.FromHours(6))
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

		private async Task HololiveCurrentLivesHandler()
		{
			while (true)
			{
				if (CurrentLiveToken.IsCancellationRequested)
					break;

				var ctx = DbService.GetContext();
				var channels = await ctx
					.Channels
					.Where(x => x.ChannelType == ChannelType.Hololive)
					.ToListAsync()
					.ConfigureAwait(false);

				var lives = await ctx
					.Lives
					.Include(x => x.Member)
					.Where(x => !x.Notified)
					.ToListAsync()
					.ConfigureAwait(false);

				Logger.Info($"Checking {lives.Count} current lives...");

				foreach (var live in lives.Where(live => (live.StartAt - DateTime.UtcNow).Duration() <= TimeSpan.FromMinutes(5)))
				{
					live.Notified = true;

					using var uow = DbService.GetUnitOfWork();
					uow.Lives.Update(live);

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
			var t = (live.StartAt - DateTime.UtcNow).Duration();
			var embed = new DiscordEmbedBuilder()
				.WithColor(DiscordColor.Red)
				.WithImageUrl(live.Cover)
				.WithTitle($"{live.Member.Name} scheduled a new live")
				.WithDescription(live.Title)
				.WithUrl($"https://www.youtube.com/watch?v={live.Room}")
				.WithAuthor(live.Member.Name, $"https://www.youtube.com/channel/{live.Member.YoutubeId}", live.Member.AvatarUrl)
				.WithFooter($"{live.StartAt.ToShortDateString()} {live.StartAt.ToShortTimeString()}", "https://png.pngtree.com/element_our/sm/20180301/sm_5a9797d5c93d3.jpg")
				.AddField("Platform", live.Platform == Platform.Youtube ? "YouTube" : "Other", true)
				.AddField("Start in", t.ToString(@"hh\:mm\:ss"), true);

			return embed.Build();
		}

		private static DiscordEmbed LiveNotificationEmbed(Live live)
		{
			var t = (live.StartAt - DateTime.UtcNow).Duration();
			var embed = new DiscordEmbedBuilder()
				.WithColor(DiscordColor.Red)
				.WithImageUrl(live.Cover)
				.WithTitle($"{live.Member.Name}'s live will be starting soon!")
				.WithDescription(live.Title)
				.WithUrl($"https://www.youtube.com/watch?v={live.Room}")
				.WithFooter($"{live.StartAt.ToShortDateString()} {live.StartAt.ToShortTimeString()}", "https://png.pngtree.com/element_our/sm/20180301/sm_5a9797d5c93d3.jpg")
				.WithAuthor(live.Member.Name, $"https://www.youtube.com/channel/{live.Member.YoutubeId}", live.Member.AvatarUrl)
				.AddField("Platform", live.Platform == Platform.Youtube ? "YouTube" : "Other", true)
				.AddField("Start in", t.ToString(@"hh\:mm\:ss"), true)
				.AddField("Mentions", "", false);

			return embed.Build();
		}
	}
}
using DSharpPlus;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Microsoft.EntityFrameworkCore;
using NLog;
using PekoBot.Core.Extensions;
using PekoBot.Core.Services;
using PekoBot.Core.Services.Interfaces;
using PekoBot.Entities.GraphQL;
using PekoBot.Entities.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PekoBot.Core.Modules.VTubers.Services
{
	public class HololiveService : IService
	{
		// ReSharper disable once InconsistentNaming
		private static GraphQLHttpClient GraphQLHttpClient { get; set; }

		private static Logger Logger { get; set; }

		private DbService DbService { get; }

		private DiscordClient Client { get; }

		private CancellationTokenSource LiveTokenSource { get; set; }
		private CancellationToken LiveToken { get; set; }

		public HololiveService(DbService dbService, DiscordClient client)
		{
			Logger = LogManager.GetCurrentClassLogger();
			GraphQLHttpClient = new GraphQLHttpClient(new GraphQLHttpClientOptions()
			{
				EndPoint = new Uri("https://api.ihateani.me/v2/graphql")

			}, new NewtonsoftJsonSerializer());

			DbService = dbService;
			Client = client;
		}

		public Task RunHandlersAsync()
		{
			LiveTokenSource = new CancellationTokenSource();
			LiveToken = LiveTokenSource.Token;

			_ = Task.Run(async () =>
			{
				try
				{
					while (true)
					{
						if (LiveToken.IsCancellationRequested)
							break;

						await UpcomingLivesHandler().ConfigureAwait(false);
						await Task.Delay(TimeSpan.FromMinutes(20), LiveToken).ConfigureAwait(false);
					}
				}
				catch (Exception e)
				{
					Logger.Error(e);
				}
			}, LiveToken);

			_ = Task.Run(async () =>
			{
				try
				{
					while (true)
					{
						if (LiveToken.IsCancellationRequested)
							break;

						await UpcomingLivesHandler().ConfigureAwait(false);
						await Task.Delay(TimeSpan.FromMinutes(20), LiveToken).ConfigureAwait(false);
					}
				}
				catch (Exception e)
				{
					Logger.Error(e);
				}
			}, LiveToken);

			return Task.CompletedTask;
		}

		public bool StopHandlers()
		{
			if (LiveTokenSource == null)
				return false;

			LiveTokenSource?.Cancel();
			return true;
		}

		private static async Task<IEnumerable<LiveObject>> GetUpcomingLivesAsync()
		{
			try
			{
				var response = await GraphQLHttpClient.SendQueryAsync<ResponseObject>(new GraphQLRequest
				{
					Query = @"query {
						vtuber { 
							upcoming(groups: [""hololive""])
							{
								items
								{
									id
									title
									thumbnail
									platform
									group
									viewers
									is_premiere
									timeData {
										scheduledStartTime
										startTime
										lateBy
										publishedAt
									}
									channel {
										name
										id
										image
									}
								}
								pageInfo {
									nextCursor
									hasNextPage
								}
							}
						}
					}
				"
				}).ConfigureAwait(false);

				return response.Data.VTuber.VTuberUpcomingLives?.Lives;
			}
			catch (Exception e)
			{
				Logger.Error(e);
				return null;
			}
		}

		private static async Task<IEnumerable<LiveObject>> GetCurrentLivesAsync()
		{
			try
			{
				var response = await GraphQLHttpClient.SendQueryAsync<ResponseObject>(new GraphQLRequest
				{
					Query = @"query {
						vtuber { 
							live(groups: [""hololive""])
							{
								items
								{
									id
									title
									thumbnail
									platform
									group
									viewers
									is_premiere
									timeData {
										scheduledStartTime
										startTime
										lateBy
										publishedAt
									}
									channel {
										name
										id
										image
									}
								}
								pageInfo {
									nextCursor
									hasNextPage
								}
							}
						}
					}
				"
				}).ConfigureAwait(false);

				return response.Data.VTuber.VTuberUpcomingLives?.Lives;
			}
			catch (Exception e)
			{
				Logger.Error(e);
				return null;
			}
		}

		private static async Task<IEnumerable<LiveObject>> GetEndedLivesAsync()
		{
			try
			{
				var response = await GraphQLHttpClient.SendQueryAsync<ResponseObject>(new GraphQLRequest
				{
					Query = @"query {
						vtuber { 
							ended(groups: [""hololive""])
							{
								items
								{
									id
									title
									thumbnail
									platform
									group
									viewers
									is_premiere
									timeData {
										scheduledStartTime
										startTime
										lateBy
										publishedAt
									}
									channel {
										name
										id
										image
									}
								}
								pageInfo {
									nextCursor
									hasNextPage
								}
							}
						}
					}
				"
				}).ConfigureAwait(false);

				return response.Data.VTuber.VTuberUpcomingLives?.Lives;
			}
			catch (Exception e)
			{
				Logger.Error(e);
				return null;
			}
		}

		private async Task UpcomingLivesHandler()
		{
			using var uow = DbService.GetUnitOfWork();
			var lives = await GetUpcomingLivesAsync().ConfigureAwait(false);

			foreach (var live in lives)
			{
				var cachedLive = await uow.Lives.GetLiveByIdAsync(live.Id).ConfigureAwait(false);

				if (cachedLive != null)
					continue;

				try
				{
					await uow.Lives.AddAsync(new Live
					{
						LiveId = live.Id,
						Title = live.Title,
						Thumbnail = live.Thumbnail,
						Platform = live.Platform.ToEnum<Platform>(),
						Viewers = live.Viewers,
						IsPremiere = live.IsPremiere,
						Status = live.Status.ToEnum<Status>(),
						ScheduledStartTime = DateTimeOffset.FromUnixTimeSeconds(live.Time.ScheduledStartTime).UtcDateTime,
						StartTime = DateTimeOffset.FromUnixTimeSeconds(live.Time.StartTime).UtcDateTime,
						EndTime = DateTimeOffset.FromUnixTimeSeconds(live.Time.EndTime).UtcDateTime,
						CreatedAt = live.Time.PublishedAt,
						LateBy = live.Time.LateBy,
						Duration = live.Time.Duration,
						Member = await uow.Members.GetByChannelIdAsync(live.Channel.Id).ConfigureAwait(false),
						Notified = false
					}).ConfigureAwait(false);
					await uow.SaveChangesAsync().ConfigureAwait(false);
				}
				catch (DbUpdateConcurrencyException ex)
				{
					Logger.Error(ex);
				}
			}
		}

		private async Task NotificationHandler()
		{
			using var uow = DbService.GetUnitOfWork();
			var lives = await uow.Lives.GetUpcomingLivesWithMember().ConfigureAwait(false);

			foreach (var live in lives)
			{
				try
				{


					live.Notified = true;
					uow.Lives.Update(live);
					await uow.SaveChangesAsync().ConfigureAwait(false);
				}
				catch (Exception e)
				{
					Logger.Error(e);
				}
			}
		}

		//private async Task HololiveCurrentLivesHandler()
		//{
		//	while (true)
		//	{
		//		if (CurrentLiveToken.IsCancellationRequested)
		//			break;

		//		var ctx = DbService.GetContext();
		//		var channels = await ctx
		//			.Channels
		//			.Where(x => x.ChannelType == ChannelType.Hololive)
		//			.ToListAsync()
		//			.ConfigureAwait(false);

		//		var lives = await ctx
		//			.Lives
		//			.Include(x => x.Member)
		//			.Where(x => !x.Notified)
		//			.ToListAsync()
		//			.ConfigureAwait(false);

		//		Logger.Info($"Checking {lives.Count} current lives...");

		//		foreach (var live in lives.Where(live => (live.StartAt - DateTime.UtcNow).Duration() <= TimeSpan.FromMinutes(5)))
		//		{
		//			live.Notified = true;

		//			using var uow = DbService.GetUnitOfWork();
		//			uow.Lives.Update(live);

		//			foreach (var channel in channels)
		//			{
		//				await Client.SendMessageAsync(await Client.GetChannelAsync(channel.ChannelId),
		//					embed: LiveNotificationEmbed(live)).ConfigureAwait(false);

		//				Thread.Sleep(TimeSpan.FromSeconds(5));
		//			}
		//		}

		//		await Task.Delay(TimeSpan.FromMinutes(5));
		//	}
		//}
	}
}
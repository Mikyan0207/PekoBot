using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using NLog.Config;
using NLog.Targets;
using PekoBot.Core.Extensions;
using PekoBot.Core.Services;
using PekoBot.Database;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NLog.Fluent;
using PekoBot.Entities.GraphQL;
using PekoBot.Entities.Models;

namespace PekoBot.Core
{
	public class PekoBot
	{
		private static Logger Logger { get; set; }

		public DiscordClient Client { get; }

		public IServiceProvider Services { get; set; }

		public CommandsNextExtension CommandsNext { get; }

		public InteractivityExtension Interactivity { get; }

		public ConfigurationService ConfigurationService { get; }

		public DbService DbService { get; }

		public PekoBot()
		{
			InitializeLogger();

			Logger = LogManager.GetCurrentClassLogger();
			ConfigurationService = new ConfigurationService();
			DbService = new DbService();

			Client = new DiscordClient(new DiscordConfiguration()
			{
				MessageCacheSize = 0,
				Token = ConfigurationService.Configuration.DiscordToken,
				AutoReconnect = true
			});

			Services = new ServiceCollection()
				.AddSingleton(Client)
				.AddSingleton(ConfigurationService)
				.LoadPekoBotServices(Assembly.GetExecutingAssembly())
				.AddSingleton(DbService)
				.AddDbContext<PekoBotContext>()
				.AddSingleton<UnitOfWork>()
				.BuildServiceProvider();

			CommandsNext = Client.UseCommandsNext(new CommandsNextConfiguration()
			{
				StringPrefixes = new[] { ConfigurationService.Configuration.Prefix },
				EnableDms = false,
				EnableMentionPrefix = false,
				EnableDefaultHelp = true,
				Services = Services
			});

			Interactivity = Client.UseInteractivity(new InteractivityConfiguration()
			{
				PaginationBehaviour = PaginationBehaviour.WrapAround,
				PaginationDeletion = PaginationDeletion.DeleteMessage,
				Timeout = TimeSpan.FromMinutes(2),
				PollBehaviour = PollBehaviour.KeepEmojis
			});

			CommandsNext.RegisterCommands(Assembly.GetExecutingAssembly());
		}

		public async Task RunAsync()
		{
			await SeedVTubersCompaniesAsync().ConfigureAwait(false);
			await SeedVTubersAsync().ConfigureAwait(false);

			await Client.ConnectAsync().ConfigureAwait(false);
			await Task.Delay(-1).ConfigureAwait(false);
		}

		private Task SeedVTubersCompaniesAsync()
		{
			Logger.Info("Loading VTubers Companies..."); // @Debug

			var ctx = DbService.GetContext();
			var content = File.ReadAllText("Resources/Companies.json");
			var companies = JsonConvert.DeserializeObject<List<Entities.Json.Companies>>(content);

			foreach (var c in companies.Where(company => !ctx.Companies.Any(x => x.Name == company.Name)))
			{
				try
				{
					ctx.Companies.Add(new Company
					{
						Name = c.Name,
						EnglishName = c.EnglishName,
						Code = c.Code,
						Image = c.Image
					});

					ctx.SaveChanges();
				}
				catch (DbUpdateConcurrencyException ex)
				{
					Logger.Error(ex);
					break;
				}
			}

			Logger.Info($"Loaded {companies.Count} VTubers Companies..."); // @Debug
			return Task.CompletedTask;
		}

		private async Task SeedVTubersAsync()
		{
			Logger.Info("Loading VTubers..."); // @Debug

			var ctx = DbService.GetContext();
			if (ctx.VTubers.Any())
			{
				Logger.Info($"Loaded {ctx.VTubers.Count()} VTubers..."); // @Debug
				await SeedEmojisAsync().ConfigureAwait(false);
				return;
			}

			using var client = new GraphQLHttpClient(new GraphQLHttpClientOptions
			{
				EndPoint = new Uri("https://api.ihateani.me/v2/graphql")
			}, new NewtonsoftJsonSerializer());
			var response = await client.SendQueryAsync<ResponseObject>(new GraphQLHttpRequest($"query {{ vtuber {{ channels {{ items {{ id user_id name en_name image platform group statistics {{ subscriberCount viewCount videoCount }} }} pageInfo {{ nextCursor hasNextPage }} }} }} }}")).ConfigureAwait(false);
			var nbr = 0;
			var uow = DbService.GetUnitOfWork();

			while (true)
			{
				var channelObject = response.Data.VTuber.VTuberChannelsObject;

				nbr += channelObject.Channels.Count;

				foreach (var ch in channelObject.Channels)
				{
					var e = await uow.VTubers.GetByNameAsync(ch.Name).ConfigureAwait(false);

					if (e != null)
						continue;

					try
					{
						var company = await uow.Companies.GetByCodeAsync(ch.Group).ConfigureAwait(false);
						
						await uow.VTubers.AddAsync(new VTuber
						{
							Name = ch.Name,
							EnglishName = ch.EnglishName,
							AvatarUrl = ch.Image,
							ChannelId = ch.Id,
							Company = company,
							Platform = ch.Platform,
							Statistics = new Statistics
							{
								SubscriberCount = ch.Statistics.SubscriberCount,
								ViewCount = ch.Statistics.ViewCount,
								VideoCount = ch.Statistics.VideoCount
							}
						}).ConfigureAwait(false);
						await uow.SaveChangesAsync().ConfigureAwait(false);
					}
					catch (DbUpdateConcurrencyException ex)
					{
						Logger.Error(ex);
						return;
					}
				}

				if (!channelObject.PageInfo.HasNextPage)
					break;

				response = await client.SendQueryAsync<ResponseObject>(new GraphQLHttpRequest($"query {{ vtuber {{ channels(cursor:\"{channelObject.PageInfo.NextCursor}\") {{ items {{ id user_id name en_name image platform group statistics {{ subscriberCount viewCount videoCount }} }} pageInfo {{ nextCursor hasNextPage }} }} }} }}")).ConfigureAwait(false);
			}

			Logger.Info($"Loaded {nbr} VTubers..."); // @Debug
			await SeedEmojisAsync().ConfigureAwait(false);
		}

		private async Task SeedEmojisAsync()
		{
			Logger.Info("Loading VTubers Emojis..."); // @Debug

			var ctx = DbService.GetContext();
			var content = await File.ReadAllTextAsync("Resources/Emojis.json").ConfigureAwait(false);
			var emojis = JsonConvert.DeserializeObject<List<Entities.Json.Emoji>>(content);

			foreach (var emoji in emojis)
			{
				if (ctx.Emojis.Include(x=>x.VTuber).Any(x => x.Code == emoji.Code && x.VTuber.EnglishName == emoji.Member))
					continue;
				try
				{
					var vtuber = await ctx.VTubers.FirstOrDefaultAsync(x => x.EnglishName == emoji.Member)
						.ConfigureAwait(false);


					if (vtuber == null)
					{
						Logger.Info("VTuber not found.");
						continue;
					}

					var e = await ctx.Emojis.AddAsync(new Emoji
					{
						Code = emoji.Code,
						VTuber = vtuber
					}).ConfigureAwait(false);
					vtuber.EmojiId = e.Entity.Id;
					ctx.VTubers.Update(vtuber);
					await ctx.SaveChangesAsync().ConfigureAwait(false);
				}
				catch (DbUpdateConcurrencyException ex)
				{
					Logger.Error(ex);
					break;
				}
			}

			Logger.Info($"Loaded {emojis.Count} VTubers Emojis..."); // @Debug
		}

		public static void InitializeLogger()
		{
			var loggingConfig = new LoggingConfiguration();
			var coloredConsoleTarget = new ColoredConsoleTarget()
			{
				Layout = "[${logger:shortName=true}] - ${longdate}\n${message}\n"
			};

			loggingConfig.AddTarget("Console", coloredConsoleTarget);
			loggingConfig.LoggingRules.Add(new LoggingRule("*", LogLevel.Info, coloredConsoleTarget));

			coloredConsoleTarget.WordHighlightingRules.Add(new ConsoleWordHighlightingRule
			{
				Regex = "\\[[^\\]]*\\]",
				ForegroundColor = ConsoleOutputColor.Cyan,
			});

			LogManager.Configuration = loggingConfig;
		}
	}
}

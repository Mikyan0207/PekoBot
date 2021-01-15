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
using System.Globalization;
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

		private async Task SeedVTubersCompaniesAsync()
		{
			Logger.Info("Loading VTuber Companies...");

			var content = await File.ReadAllTextAsync("Resources/Companies.json").ConfigureAwait(false);
			var companies = JsonConvert.DeserializeObject<List<Entities.Json.Companies>>(content);

			using var uow = DbService.GetUnitOfWork();

			foreach (var c in companies)
			{
				try
				{
					if (await uow.Companies.AnyAsync(x => x.Code == c.Code).ConfigureAwait(false))
						continue;

					await uow.Companies.AddAsync(new Company
					{
						Name = c.Name,
						EnglishName = c.EnglishName,
						Code = c.Code,
						Image = c.Image
					}).ConfigureAwait(false);
					await uow.SaveChangesAsync().ConfigureAwait(false);
				}
				catch (DbUpdateConcurrencyException e)
				{
					Logger.Error(e);
				}
			}

			Logger.Info($"Loaded {companies.Count} VTuber Companies...");
		}

		private async Task SeedVTubersAsync()
		{
			Logger.Info("Loading VTubers...");

			var content = await File.ReadAllTextAsync("Resources/VTubers.json").ConfigureAwait(false);
			var vtubers = JsonConvert.DeserializeObject<List<Entities.Json.VTuber>>(content);
			
			using var uow = DbService.GetUnitOfWork();

			foreach (var vtuber in vtubers)
			{
				try
				{
					if (await uow.VTubers.AnyAsync(x => x.Name == vtuber.Name).ConfigureAwait(false))
						continue;

					await uow.VTubers.AddAsync(new VTuber
					{
						Name = vtuber.Name,
						EnglishName = vtuber.EnglishName,
						Nicknames = vtuber.Nicknames.ToArray(),
						DebutDate = DateTime.Parse(vtuber.DebutDate, new CultureInfo("jp"), DateTimeStyles.None),
						Company = await uow.Companies.GetByCodeAsync(vtuber.Company).ConfigureAwait(false),
						Generation = vtuber.Generation,
						Accounts = vtuber.Accounts.Select(x => new Account
						{
							Name = x.Name,
							AccountId = x.AccountId,
							Image = x.Image,
							Platform = x.Platform.ToEnum<Platform>(),
							Statistics = new Statistics
							{
								SubscriberCount = x.Statistics.SubscriberCount,
								ViewCount = x.Statistics.ViewCount,
								VideoCount = x.Statistics.VideoCount
							}
						}).ToList(),
						Emoji = new Emoji
						{
							Code = vtuber.Emoji
						}
					}).ConfigureAwait(false);
					await uow.SaveChangesAsync().ConfigureAwait(false);
				}
				catch (DbUpdateConcurrencyException e)
				{
					Logger.Error(e);
				}
			}

			Logger.Info($"Loaded {vtubers.Count} VTubers...");
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

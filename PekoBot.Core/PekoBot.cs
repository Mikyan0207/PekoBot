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
using System.Reflection;
using System.Threading.Tasks;

namespace PekoBot.Core
{
	public class PekoBot
	{
		public DiscordClient Client { get; }

		public IServiceProvider Services { get; set; }

		public CommandsNextExtension CommandsNext { get; }

		public InteractivityExtension Interactivity { get; }

		public ConfigurationService ConfigurationService { get; }

		public DbService DbService { get; }

		public PekoBot()
		{
			InitializeLogger();
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

			Client.MessageReactionAdded += ClientOnMessageReactionAdded;
		}

		private async Task ClientOnMessageReactionAdded(DiscordClient sender, MessageReactionAddEventArgs e)
		{
			using var uow = DbService.GetUnitOfWork();
			var message = uow.Messages.GetMessageById(e.Message.Id);

			if (message == null)
				return;


		}

		public async Task RunAsync()
		{
			await Client.ConnectAsync().ConfigureAwait(false);
			await Task.Delay(-1).ConfigureAwait(false);
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

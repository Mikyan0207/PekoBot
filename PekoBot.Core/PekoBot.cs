using System;
using System.Reflection;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using NLog.Config;
using NLog.Targets;
using PekoBot.Core.Extensions;
using PekoBot.Core.Modules.VTubers.Common;
using PekoBot.Core.Modules.VTubers.Services;
using PekoBot.Core.Services.Impl;

namespace PekoBot.Core
{
	public class PekoBot
	{
		public DiscordClient Client { get; }

		public IServiceProvider Services { get; set; }

		public CommandsNextExtension CommandsNext { get; }

		public InteractivityExtension Interactivity { get; }

		public ConfigurationService ConfigurationService { get; }

		public PekoBot()
		{
            InitializeLogger();
			ConfigurationService = new ConfigurationService();

			Client = new DiscordClient(new DiscordConfiguration()
			{
				MessageCacheSize = 0,
				Token = ConfigurationService.Configuration.DiscordToken,
				AutoReconnect = true
			});

			Services = new ServiceCollection()
				.AddSingleton(Client)
				.AddSingleton(ConfigurationService)
				.AddSingleton<DbService>()
				.LoadPekoBotServices(Assembly.GetCallingAssembly())
				.BuildServiceProvider();

			CommandsNext = Client.UseCommandsNext(new CommandsNextConfiguration()
			{
				StringPrefixes = new[] { ConfigurationService.Configuration.Prefix },
				EnableDms = false,
				EnableMentionPrefix = false,
				EnableDefaultHelp = true
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
			await Client.ConnectAsync().ConfigureAwait(false);

			await Services.GetService<HololiveService>().RunHandlersAsync().ConfigureAwait(false);

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

            coloredConsoleTarget.WordHighlightingRules.Add(new ConsoleWordHighlightingRule
            {
                Text = "Guild Joined",
                ForegroundColor = ConsoleOutputColor.Green,
            });

            coloredConsoleTarget.WordHighlightingRules.Add(new ConsoleWordHighlightingRule
            {
                Text = "Guild Left",
                ForegroundColor = ConsoleOutputColor.Blue,
            });

            coloredConsoleTarget.WordHighlightingRules.Add(new ConsoleWordHighlightingRule
            {
                Text = "Guild Available",
                ForegroundColor = ConsoleOutputColor.Yellow
            });

            coloredConsoleTarget.WordHighlightingRules.Add(new ConsoleWordHighlightingRule
            {
                Text = "Name",
                ForegroundColor = ConsoleOutputColor.Red,
            });

            coloredConsoleTarget.WordHighlightingRules.Add(new ConsoleWordHighlightingRule
            {
                Text = "Owner",
                ForegroundColor = ConsoleOutputColor.Red,
            });

            coloredConsoleTarget.WordHighlightingRules.Add(new ConsoleWordHighlightingRule
            {
                Text = "Members",
                ForegroundColor = ConsoleOutputColor.Red,
            });

            coloredConsoleTarget.WordHighlightingRules.Add(new ConsoleWordHighlightingRule
            {
                Text = "Created",
                ForegroundColor = ConsoleOutputColor.Red,
            });

            coloredConsoleTarget.WordHighlightingRules.Add(new ConsoleWordHighlightingRule
            {
                Text = "User",
                ForegroundColor = ConsoleOutputColor.Red,
            });

            coloredConsoleTarget.WordHighlightingRules.Add(new ConsoleWordHighlightingRule
            {
                Text = "Channel",
                ForegroundColor = ConsoleOutputColor.Red,
            });

            coloredConsoleTarget.WordHighlightingRules.Add(new ConsoleWordHighlightingRule
            {
                Text = "Guild",
                ForegroundColor = ConsoleOutputColor.Red,
            });

            coloredConsoleTarget.WordHighlightingRules.Add(new ConsoleWordHighlightingRule
            {
                Text = "Date",
                ForegroundColor = ConsoleOutputColor.Red,
            });

            coloredConsoleTarget.WordHighlightingRules.Add(new ConsoleWordHighlightingRule
            {
                Text = "Raw Message",
                ForegroundColor = ConsoleOutputColor.Red,
            });

            LogManager.Configuration = loggingConfig;
        }
    }
}

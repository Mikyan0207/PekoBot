﻿using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using PekoBot.Core.Modules.VTubers.Services;
using PekoBot.Entities.Models;

namespace PekoBot.Core.Modules.VTubers
{
	[Group("hl")]
	[Description("hololive related commands.")]
	public sealed class HololiveModule : PekoModule
	{
		private HololiveService HololiveService { get; }

		public HololiveModule(HololiveService hololiveService)
		{
			HololiveService = hololiveService;
		}

		[Command("start")]
		[Description("Start Notification System")]
		[RequireOwner]
		public async Task Start(CommandContext ctx)
		{
			await HololiveService.RunHandlersAsync().ConfigureAwait(false);

			await EmbedAsync(ctx, new DiscordEmbedBuilder()
				.WithTitle("Success")
				.WithDescription("Started Notification System.")
				.WithColor(DiscordColor.SpringGreen)).ConfigureAwait(false);
		}

		[Command("stop")]
		[Description("Stop Notification System")]
		[RequireOwner]
		public async Task Stop(CommandContext ctx)
		{
			var result = HololiveService.StopHandlers();

			if (result)
			{
				await EmbedAsync(ctx, new DiscordEmbedBuilder()
					.WithTitle("Success")
					.WithDescription("Stopped Notification System.")
					.WithColor(DiscordColor.SpringGreen)).ConfigureAwait(false);
			}
			else
			{
				await EmbedAsync(ctx, new DiscordEmbedBuilder()
					.WithTitle("Error")
					.WithDescription("Notification System already stopped.")
					.WithColor(DiscordColor.IndianRed)).ConfigureAwait(false);
			}
		}
	}
}
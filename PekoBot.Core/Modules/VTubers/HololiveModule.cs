using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Microsoft.EntityFrameworkCore;
using NLog;
using PekoBot.Core.Modules.VTubers.Services;
using PekoBot.Core.Services.Impl;
using PekoBot.Entities.Models;
using ChannelType = PekoBot.Entities.Enums.ChannelType;

namespace PekoBot.Core.Modules.VTubers
{
	[Group("hl")]
	[Description("hololive related commands.")]
	public sealed class HololiveModule : PekoModule
	{
		private Logger Logger { get; }

		private HololiveService HololiveService { get; }
		private DbService DbService { get; }

		public HololiveModule(HololiveService hololiveService, DbService dbService)
		{
			Logger = LogManager.GetCurrentClassLogger();
			HololiveService = hololiveService;
			DbService = dbService;
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

		[Command("enable")]
		[Description("Enable hololive notifications in this channel.")]
		[RequireUserPermissions(Permissions.ManageGuild)]
		[RequireBotPermissions(Permissions.ManageChannels)]
		public async Task EnableNotificationAsync(CommandContext ctx)
		{
			using var uow = DbService.GetUnitOfWork();
			var ch = await uow.Channels.GetChannelById(ctx.Channel.Id).ConfigureAwait(false);

			if (ch == null || ch?.ChannelType != ChannelType.Hololive)
			{
				try
				{
					ch = new Channel()
					{
						ChannelId = ctx.Channel.Id,
						ChannelName = ctx.Channel.Name,
						ChannelType = ChannelType.Hololive
					};

					await uow.Channels.AddAsync(ch).ConfigureAwait(false);
					await uow.SaveChangesAsync().ConfigureAwait(false);

					await EmbedAsync(ctx, new DiscordEmbedBuilder()
						.WithTitle("Success")
						.WithDescription("hololive notifications successfully enabled.")
						.WithColor(DiscordColor.SpringGreen)).ConfigureAwait(false);

					await Start(ctx).ConfigureAwait(false);
				}
				catch (DbUpdateConcurrencyException e)
				{
					Logger.Error(e);
				}
			}
			else if (ch.ChannelType == ChannelType.Hololive)
			{
				await EmbedAsync(ctx, new DiscordEmbedBuilder()
					.WithTitle("Error")
					.WithDescription("This channel already receives hololive notifications.")
					.WithColor(DiscordColor.IndianRed)).ConfigureAwait(false);
			}
		}

		[Command("disable")]
		[Description("Disable hololive notifications in this channel.")]
		public async Task DisableNotificationAsync(CommandContext ctx)
		{
			using var uow = DbService.GetUnitOfWork();
			var ch = await uow.Channels.GetChannelById(ctx.Channel.Id).ConfigureAwait(false);

			if (ch == null || ch?.ChannelType != ChannelType.Hololive)
			{
				await EmbedAsync(ctx, new DiscordEmbedBuilder()
					.WithTitle("Error")
					.WithDescription("hololive notifications aren't enabled on this channel.")
					.WithColor(DiscordColor.IndianRed)).ConfigureAwait(false);
			}
			else
			{
				try
				{
					uow.Channels.Remove(ch);
					await uow.SaveChangesAsync().ConfigureAwait(false);

					await EmbedAsync(ctx, new DiscordEmbedBuilder()
						.WithTitle("Success")
						.WithDescription("hololive notifications successfully disabled.")
						.WithColor(DiscordColor.SpringGreen)).ConfigureAwait(false);
				}
				catch (DbUpdateConcurrencyException e)
				{
					Logger.Error(e);
				}
			}
		}
	}
}
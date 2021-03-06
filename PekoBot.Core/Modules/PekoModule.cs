﻿using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using System;
using System.Threading.Tasks;

namespace PekoBot.Core.Modules
{
	public class PekoModule : BaseCommandModule
	{
		protected virtual async Task<DiscordMessage> EmbedAsync(CommandContext ctx, DiscordEmbedBuilder eb,
			string message = null)
		{
			await ctx.Channel.TriggerTypingAsync().ConfigureAwait(false);

			return await ctx.Channel.SendMessageAsync(message ?? "", false, eb).ConfigureAwait(false);
		}

		protected virtual async Task<DiscordMessage> SendErrorAsync(CommandContext ctx, string title, string error)
		{
			var embedBuilder = new DiscordEmbedBuilder()
				.WithTitle(title)
				.WithDescription(error)
				.WithTimestamp(DateTime.Now)
				.WithColor(DiscordColor.Red);

			return await EmbedAsync(ctx, embedBuilder).ConfigureAwait(false);
		}

		protected virtual async Task<DiscordMessage> SendConfirmationAsync(CommandContext ctx, string title, string message)
		{
			var embedBuilder = new DiscordEmbedBuilder()
				.WithTitle(title)
				.WithDescription(message)
				.WithTimestamp(DateTime.Now)
				.WithColor(DiscordColor.SpringGreen);

			return await EmbedAsync(ctx, embedBuilder).ConfigureAwait(false);
		}
	}
}

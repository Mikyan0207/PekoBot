using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using NLog;
using PekoBot.Core.Services;

namespace PekoBot.Core.Modules.VTubers
{
	[Group("vt")]
	[Description("VTubers related commands.")]
	public class VTubersModule : PekoModule
	{
		private Logger Logger { get; }
		private DbService DbService { get; }

		public VTubersModule(DbService dbService)
		{
			Logger = LogManager.GetCurrentClassLogger();
			DbService = dbService;
		}

		[Command("info")]
		[Description("Get information about a VTuber.")]
		public async Task InfoAsync(CommandContext ctx, [RemainingText] string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				await SendErrorAsync(ctx, "Error", "VTuber not found.").ConfigureAwait(false);
				return;
			}

			using var uow = DbService.GetUnitOfWork();
			var vt = await uow.VTubers.GetByNameAsync(name).ConfigureAwait(false);

			if (vt == null)
			{
				await SendErrorAsync(ctx, "Error", "VTuber not found.").ConfigureAwait(false);
				return;
			}

			var embed = new DiscordEmbedBuilder()
				.WithAuthor(vt.Name, $"https://www.youtube.com/channel/{vt.ChannelId}", vt.AvatarUrl)
				.WithDescription(vt.EnglishName)
				.WithColor(DiscordColor.SpringGreen)
				.AddField("Company", $"{(vt.Company != null ? vt.Company.Name : "None")}", true)
				.AddField("Emoji", $"{(vt.Emoji != null ? vt.Emoji.Code : "None")}")
				.AddField("Subscribers", $"{vt.Statistics.SubscriberCount}", true)
				.AddField("Views", $"{vt.Statistics.ViewCount}", true)
				.AddField("Video", $"{vt.Statistics.VideoCount}", true)
				.WithTimestamp(DateTime.UtcNow);

			if (vt.Company != null)
				embed.WithThumbnail(new Uri(vt.Company.Image), 5, 5);

			await EmbedAsync(ctx, embed).ConfigureAwait(false);
		}
	}
}
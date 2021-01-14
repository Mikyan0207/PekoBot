using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity.Extensions;
using Microsoft.EntityFrameworkCore;
using NLog;
using PekoBot.Core.Services;

namespace PekoBot.Core.Modules.Roles
{
	[Group("r")]
	public class RolesModule : PekoModule
	{
		private static Logger Logger { get; } = LogManager.GetCurrentClassLogger();

		private DbService DbService { get; }
		
		private DiscordClient Client { get;}

		public RolesModule(DbService dbService, DiscordClient client)
		{
			DbService = dbService;
			Client = client;
		}

		[Command("hololive")]
		[RequireBotPermissions(Permissions.AddReactions | Permissions.ManageChannels)]
		[RequireUserPermissions(Permissions.ManageGuild)]
		public async Task VTuberRolesAsync(CommandContext ctx)
		{
			var context = DbService.GetContext();
			var interactivity = Client.GetInteractivity();

			var hololiveVTubers = await context
				.VTubers
				.Include(x => x.Company)
				.Include(x => x.Emoji)
				.Where(x => x.Company.Code == "hololive")
				.ToListAsync()
				.ConfigureAwait(false);

			var sb = new StringBuilder();

			foreach (var vtuber in hololiveVTubers)
				sb.AppendLine($"{vtuber?.Emoji?.Code}  {vtuber?.Name} ({vtuber?.EnglishName})");

			await ctx.Channel.SendPaginatedMessageAsync(ctx.Member, interactivity.GeneratePagesInEmbed(sb.ToString()))
				.ConfigureAwait(false);
		}
	}
}
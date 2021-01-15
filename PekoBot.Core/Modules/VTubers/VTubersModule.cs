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
		}
	}
}
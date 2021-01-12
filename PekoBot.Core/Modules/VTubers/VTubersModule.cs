using DSharpPlus.CommandsNext.Attributes;
using NLog;
using PekoBot.Core.Services;

namespace PekoBot.Core.Modules.VTubers
{
	[Group("vt")]
	[Description("VTubers related commands.")]
	public class VTubersModule : PekoModule
	{
		[Group("roles")]
		[Description("Roles configuration for VTubers commands.")]
		public class Roles : PekoModule
		{
			private Logger Logger { get; }
			private DbService DbService { get; }

			public Roles(DbService dbService)
			{
				Logger = LogManager.GetCurrentClassLogger();
				DbService = dbService;
			}
		}
	}
}
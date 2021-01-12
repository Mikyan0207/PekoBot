using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Microsoft.EntityFrameworkCore;
using NLog;
using PekoBot.Core.Services.Impl;
using PekoBot.Entities.Models;

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
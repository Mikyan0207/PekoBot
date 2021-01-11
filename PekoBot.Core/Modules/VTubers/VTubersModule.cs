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

			[Command("link")]
			[Description("Link a VTuber with a role used for mentions.")]
			public async Task LinkAsync(CommandContext ctx,
				[Description("Role to link with a specific VTuber.")] DiscordRole role,
				[Description("VTuber name to link with a specific Role.")][RemainingText] string vtuber)
			{
				if (string.IsNullOrWhiteSpace(vtuber))
				{
					await SendErrorAsync(ctx, "Error", "VTuber name not specified.").ConfigureAwait(false);
					return;
				}

				using var uow = DbService.GetUnitOfWork();
				var r = await uow.Roles.GetRoleByIdAsync(role.Id).ConfigureAwait(false);

				if (r?.Member != null)
				{
					await SendErrorAsync(ctx, "Error", $"This role is already linked with the VTuber `{r.Member.Name}`.")
						.ConfigureAwait(false);
					return;
				}

				Member v = null;

				try
				{
					v = await uow.Members.GetByNameAsync(vtuber).ConfigureAwait(false);
				}
				catch (Exception e)
				{
					Logger.Error(e);
				}

				if (v == null)
				{
					await SendErrorAsync(ctx, "Error", $"VTuber `{vtuber}` not found.")
						.ConfigureAwait(false);
					return;
				}
				if (v.Role != null)
				{
					await SendErrorAsync(ctx, "Error", $"The VTuber `{v.Name}` is already linked with the role `{v.Role.Name}`.")
						.ConfigureAwait(false);
					return;
				}

				try
				{
					if (r == null)
					{
						var newRole = await uow.Roles.AddAsync(new Role()
						{
							Name = role.Name,
							RoleId = role.Id,
							Color = role.Color.ToString(),
							Member = v
						}).ConfigureAwait(false);

						v.Role = newRole;

						uow.Members.Update(v);
						await uow.SaveChangesAsync().ConfigureAwait(false);
						await SendConfirmationAsync(ctx, "Role Linked", $"Role `{newRole.Name}` linked with VTuber `{v.Name}`");
					}
					else
					{
						r.Member = v;
						v.Role = r;

						uow.Members.Update(v);
						uow.Roles.Update(r);
						await uow.SaveChangesAsync().ConfigureAwait(false);
						await SendConfirmationAsync(ctx, "Role Linked", $"Role {r.Name} linked with VTuber {v.Name}");
					}

				}
				catch (DbUpdateConcurrencyException e)
				{
					Logger.Error(e);
				}
			}
		}
	}
}
using System;
using Microsoft.EntityFrameworkCore;
using PekoBot.Database.Repositories.Interfaces;
using PekoBot.Entities.Models;
using System.Threading.Tasks;
using NLog;

namespace PekoBot.Database.Repositories
{
	public class GuildRepository : Repository<Guild>, IGuildRepository
	{
		private static Logger Logger { get; } = LogManager.GetCurrentClassLogger();

		public GuildRepository(PekoBotContext context) : base(context)
		{

		}

		public async Task<Guild> GetByIdAsync(ulong id)
		{
			return await Context.Guilds.FirstOrDefaultAsync(x => x.GuildId == id).ConfigureAwait(false);
		}

		public async Task RemoveByIdAsync(ulong id)
		{
			try
			{
				var g = await GetByIdAsync(id).ConfigureAwait(false);

				if (g != null)
				{
					Context.Guilds.Remove(g);
					await Context.SaveChangesAsync().ConfigureAwait(false);
				}
			}
			catch (DbUpdateConcurrencyException e)
			{
				Logger.Error(e);
			}
		}
	}
}
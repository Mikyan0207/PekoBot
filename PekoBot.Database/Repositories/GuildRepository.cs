using PekoBot.Database.Repositories.Interfaces;
using PekoBot.Entities.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PekoBot.Database.Repositories
{
	public class GuildRepository : Repository<Guild>, IGuildRepository
	{
		public GuildRepository(PekoBotContext context) : base(context)
		{
			
		}

		public async Task<Guild> GetByIdAsync(ulong id)
		{
			return await Context.Guilds.FirstOrDefaultAsync(x => x.GuildId == id).ConfigureAwait(false);
		}
	}
}
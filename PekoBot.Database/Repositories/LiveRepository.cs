using PekoBot.Database.Repositories.Interfaces;
using PekoBot.Entities;
using PekoBot.Entities.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PekoBot.Database.Repositories
{
	public class LiveRepository : Repository<Live>, ILiveRepository
	{
		public LiveRepository(PekoBotContext context) : base(context)
		{
		}

		public async Task<Live> GetLiveByIdAsync(int id)
		{
			return await Context.Lives.FirstOrDefaultAsync(x => x.LiveId == id).ConfigureAwait(false);
		}
	}
}
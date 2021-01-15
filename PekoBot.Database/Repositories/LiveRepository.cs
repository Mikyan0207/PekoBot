using Microsoft.EntityFrameworkCore;
using PekoBot.Database.Repositories.Interfaces;
using PekoBot.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PekoBot.Database.Repositories
{
	public class LiveRepository : Repository<Live>, ILiveRepository
	{
		public LiveRepository(PekoBotContext context) : base(context)
		{
		}

		public async Task<Live> GetLiveByIdAsync(string id)
		{
			return await Context.Lives.FirstOrDefaultAsync(x => x.LiveId == id).ConfigureAwait(false);
		}

		public async Task<IEnumerable<Live>> GetUpcomingLives()
		{
			return await Context.Lives
				.Include(x => x.VTuber)
					.ThenInclude(y => y.Company)
				.Include(x => x.VTuber)
					.ThenInclude(y => y.Accounts)
				.Where(x => !x.Notified)
				.ToListAsync()
				.ConfigureAwait(false);
		}
	}
}
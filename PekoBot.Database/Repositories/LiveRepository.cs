﻿using Microsoft.EntityFrameworkCore;
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

		public async Task<IEnumerable<Live>> GetUpcomingLivesWithMember()
		{
			return await Context.Lives
				.Include(x => x.VTuber)
					.ThenInclude(y => y.Roles)
				.Include(x => x.VTuber)
					.ThenInclude(y => y.Company)
				.Where(x => !x.Notified && (x.ScheduledStartTime - DateTime.UtcNow) <= TimeSpan.FromMinutes(15))
				.ToListAsync()
				.ConfigureAwait(false);
		}
	}
}
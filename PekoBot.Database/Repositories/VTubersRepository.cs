using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PekoBot.Database.Repositories.Interfaces;
using PekoBot.Entities.Models;

namespace PekoBot.Database.Repositories
{
	public class VTubersRepository : Repository<VTuber>, IVTubersRepository
	{
		public VTubersRepository(PekoBotContext context) : base(context)
		{
		}

		public async Task<VTuber> GetByChannelIdAsync(string channelId)
		{
			return await Context
				.VTubers
				.FirstOrDefaultAsync(x => x.YoutubeId == channelId)
				.ConfigureAwait(false);
		}

		public async Task<VTuber> GetByNameAsync(string name)
		{
			return await Context
				.VTubers
				.FirstOrDefaultAsync(x=>x.Name == name)
				.ConfigureAwait(false);
		}
	}
}
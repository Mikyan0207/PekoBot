using Microsoft.EntityFrameworkCore;
using PekoBot.Database.Repositories.Interfaces;
using PekoBot.Entities.Models;
using System.Threading.Tasks;

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
				.FirstOrDefaultAsync(x => x.ChannelId == channelId)
				.ConfigureAwait(false);
		}

		public async Task<VTuber> GetByNameAsync(string name)
		{
			return await Context
				.VTubers
				.FirstOrDefaultAsync(x => x.Name == name)
				.ConfigureAwait(false);
		}

		public async Task<VTuber> GetOrCreate(string name)
		{
			var vtuber = await Context.VTubers.FirstOrDefaultAsync(x => x.Name == name).ConfigureAwait(false);

			if (vtuber != null)
				return vtuber;

			var e = await Context.VTubers.AddAsync(new VTuber
			{
				Name = name
			}).ConfigureAwait(false);

			return e.Entity;
		}
	}
}
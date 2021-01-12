using PekoBot.Database.Repositories.Interfaces;
using PekoBot.Entities.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace PekoBot.Database.Repositories
{
	public class ChannelRepository : Repository<Channel>, IChannelRepository
	{
		public ChannelRepository(PekoBotContext context) : base(context)
		{
		}

		public async Task<Channel> GetChannelById(ulong channelId)
		{
			return await Context
				.Channels
				.FirstOrDefaultAsync(x => x.ChannelId == channelId)
				.ConfigureAwait(false);
		}

		public async Task<List<Channel>> GetChannelsByType(string type)
		{
			return await Context
				.Channels
				.Where(x => x.ChannelType == type)
				.ToListAsync()
				.ConfigureAwait(false);
		}

		public async Task<List<Channel>> GetChannelsByTypes(string[] types)
		{
			return await Context
				.Channels
				.Where(x => types.Any(t => t == x.ChannelType))
				.ToListAsync()
				.ConfigureAwait(false);
		}

		public async Task<Channel> GetChannelWithMessages(ulong channelId)
		{
			return await Context
				.Channels
				.Include(x => x.Guild)
				.Include(x => x.Messages)
				.FirstOrDefaultAsync(x => x.ChannelId == channelId)
				.ConfigureAwait(false);
		}
	}
}
using PekoBot.Database.Repositories.Interfaces;
using PekoBot.Entities.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PekoBot.Database.Repositories
{
	public class ChannelRepository : Repository<Channel>, IChannelRepository
	{
		public ChannelRepository(PekoBotContext context) : base(context)
		{
		}

		public async Task<Channel> GetChannelById(ulong channelId)
		{
			return await Context.Channels.FirstOrDefaultAsync(x => x.ChannelId == channelId).ConfigureAwait(false);
		}
	}
}
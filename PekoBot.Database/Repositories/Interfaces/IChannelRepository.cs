using System.Threading.Tasks;
using PekoBot.Entities.Models;

namespace PekoBot.Database.Repositories.Interfaces
{
	public interface IChannelRepository : IRepository<Channel>
	{
		public Task<Channel> GetChannelById(ulong channelId);
	}
}
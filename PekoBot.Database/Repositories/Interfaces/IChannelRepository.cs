using PekoBot.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PekoBot.Database.Repositories.Interfaces
{
	public interface IChannelRepository : IRepository<Channel>
	{
		public Task<Channel> GetChannelById(ulong channelId);

		public Task<List<Channel>> GetChannelsByType(string type);

		public Task<List<Channel>> GetChannelsByTypes(string[] types);

		public Task<Channel> GetChannelWithMessages(ulong channelId);
	}
}
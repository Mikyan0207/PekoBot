using System.Threading.Tasks;
using PekoBot.Entities.Models;

namespace PekoBot.Database.Repositories.Interfaces
{
	public interface IMembersRepository : IRepository<Member>
	{
		public Task<Member> GetByChannelIdAsync(string channelId);

		public Task<Member> GetByNameAsync(string name);
	}
}
using System.Threading.Tasks;
using PekoBot.Entities.Models;

namespace PekoBot.Database.Repositories.Interfaces
{
	public interface IMemberRepository : IRepository<Member>
	{
		public Task<Member> GetByChannelIdAsync(string channelId);
	}
}
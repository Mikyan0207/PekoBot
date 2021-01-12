using System.Threading.Tasks;
using PekoBot.Entities.Models;

namespace PekoBot.Database.Repositories.Interfaces
{
	public interface IVTubersRepository : IRepository<VTuber>
	{
		public Task<VTuber> GetByChannelIdAsync(string channelId);

		public Task<VTuber> GetByNameAsync(string name);
	}
}
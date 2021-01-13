using PekoBot.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PekoBot.Database.Repositories.Interfaces
{
	public interface ILiveRepository : IRepository<Live>
	{
		public Task<Live> GetLiveByIdAsync(string id);

		public Task<IEnumerable<Live>> GetUpcomingLivesWithMember();
	}
}
using System.Collections.Generic;
using System.Threading.Tasks;
using PekoBot.Entities;
using PekoBot.Entities.Models;

namespace PekoBot.Database.Repositories.Interfaces
{
	public interface ILiveRepository : IRepository<Live>
	{
		public Task<Live> GetLiveByIdAsync(string id);

		public Task<IEnumerable<Live>> GetUpcomingLivesWithMember();
	}
}
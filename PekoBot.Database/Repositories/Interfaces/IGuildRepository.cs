using System.Threading.Tasks;
using PekoBot.Entities.Models;

namespace PekoBot.Database.Repositories.Interfaces
{
	public interface IGuildRepository : IRepository<Guild>
	{
		public Task<Guild> GetByIdAsync(ulong id);
	}
}
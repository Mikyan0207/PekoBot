using PekoBot.Entities.Models;
using System.Threading.Tasks;

namespace PekoBot.Database.Repositories.Interfaces
{
	public interface IGuildRepository : IRepository<Guild>
	{
		public Task<Guild> GetByIdAsync(ulong id);

		public Task RemoveByIdAsync(ulong id);
	}
}
using PekoBot.Entities.Models;
using System.Threading.Tasks;

namespace PekoBot.Database.Repositories.Interfaces
{
	public interface IRolesRepository : IRepository<Role>
	{
		public Task<Role> GetRoleByIdAsync(ulong id);
	}
}
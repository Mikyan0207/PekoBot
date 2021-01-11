using System.Threading.Tasks;
using PekoBot.Entities.Models;

namespace PekoBot.Database.Repositories.Interfaces
{
	public interface IRolesRepository : IRepository<Role>
	{
		public Task<Role> GetRoleByIdAsync(ulong id);
	}
}
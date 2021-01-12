using PekoBot.Database.Repositories.Interfaces;
using PekoBot.Entities.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PekoBot.Database.Repositories
{
	public class RolesRepository : Repository<Role>, IRolesRepository
	{
		public RolesRepository(PekoBotContext context) : base(context)
		{
		}

		public async Task<Role> GetRoleByIdAsync(ulong id)
		{
			return await Context
				.Roles
				.Include(x => x.VTuber)
				.FirstOrDefaultAsync(x => x.RoleId == id)
				.ConfigureAwait(false);
		}
	}
}
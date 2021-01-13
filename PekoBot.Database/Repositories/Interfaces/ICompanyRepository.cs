using PekoBot.Entities.Models;
using System.Threading.Tasks;

namespace PekoBot.Database.Repositories.Interfaces
{
	public interface ICompanyRepository : IRepository<Company>
	{
		public Task<Company> GetOrCreate(string name);

		public Task<Company> GetByNameAsync(string name);
	}
}
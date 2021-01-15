using System.Collections.Generic;
using PekoBot.Entities.Models;
using System.Threading.Tasks;

namespace PekoBot.Database.Repositories.Interfaces
{
	public interface IVTubersRepository : IRepository<VTuber>
	{
		public Task<VTuber> GetOrCreate(string name);

		public Task<VTuber> GetByChannelIdAsync(string channelId);

		public Task<VTuber> GetByNameAsync(string name);

		public Task<List<VTuber>> GetByCompanyAsync(string company);
	}
}
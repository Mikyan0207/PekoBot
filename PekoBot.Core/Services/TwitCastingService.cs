using System.Threading.Tasks;
using PekoBot.Core.Services.Interfaces;

namespace PekoBot.Core.Services
{
	public class TwitCastingService : IService
	{
		private DbService DbService { get; }

		public TwitCastingService(DbService dbService)
		{
			DbService = dbService;
		}

		public async Task UpdateStatisticsAsync()
		{
		}
	}
}
using System.Threading.Tasks;
using PekoBot.Core.Services.Interfaces;

namespace PekoBot.Core.Services
{
	public class TwitchService : IService
	{
		private DbService DbService { get; }

		public TwitchService(DbService dbService)
		{
			DbService = dbService;
		}

		public async Task UpdateStatisticsAsync()
		{
		}
	}
}
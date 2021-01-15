using System.Threading.Tasks;
using PekoBot.Core.Services.Interfaces;

namespace PekoBot.Core.Services
{
	public class YouTubeService : IService
	{
		private DbService DbService { get; }

		public YouTubeService(DbService dbService)
		{
			DbService = dbService;
		}

		public async Task UpdateStatisticsAsync()
		{

		}
	}
}
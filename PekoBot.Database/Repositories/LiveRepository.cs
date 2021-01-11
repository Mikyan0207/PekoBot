using PekoBot.Database.Repositories.Interfaces;
using PekoBot.Entities;
using PekoBot.Entities.Models;
using System.Threading.Tasks;

namespace PekoBot.Database.Repositories
{
	public class LiveRepository : Repository<Live>, ILiveRepository
	{
		public LiveRepository(PekoBotContext context) : base(context)
		{
		}
	}
}
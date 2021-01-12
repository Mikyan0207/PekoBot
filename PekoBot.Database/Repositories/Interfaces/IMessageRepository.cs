using System.Threading.Tasks;
using PekoBot.Entities.Models;

namespace PekoBot.Database.Repositories.Interfaces
{
	public interface IMessageRepository : IRepository<Message>
	{
		public Task<Message> GetMessageById(ulong messageId);
	}
}
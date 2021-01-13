using Microsoft.EntityFrameworkCore;
using PekoBot.Database.Repositories.Interfaces;
using PekoBot.Entities.Models;
using System.Threading.Tasks;

namespace PekoBot.Database.Repositories
{
	public class MessageRepository : Repository<Message>, IMessageRepository
	{
		public MessageRepository(PekoBotContext context) : base(context)
		{
		}

		public async Task<Message> GetMessageById(ulong messageId)
		{
			return await Context
				.Messages
				.FirstOrDefaultAsync(x => x.MessageId == messageId)
				.ConfigureAwait(false);
		}
	}
}
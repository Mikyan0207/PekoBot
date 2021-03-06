﻿using PekoBot.Entities.Models;
using System.Threading.Tasks;

namespace PekoBot.Database.Repositories.Interfaces
{
	public interface IMessageRepository : IRepository<Message>
	{
		public Task<Message> GetMessageById(ulong messageId);
	}
}
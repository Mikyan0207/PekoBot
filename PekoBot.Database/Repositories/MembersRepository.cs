using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PekoBot.Database.Repositories.Interfaces;
using PekoBot.Entities.Models;

namespace PekoBot.Database.Repositories
{
	public class MembersRepository : Repository<Member>, IMembersRepository
	{
		public MembersRepository(PekoBotContext context) : base(context)
		{
		}

		public async Task<Member> GetByChannelIdAsync(string channelId)
		{
			return await Context
				.Members
				.FirstOrDefaultAsync(x => x.YoutubeId == channelId)
				.ConfigureAwait(false);
		}

		public async Task<Member> GetByNameAsync(string name)
		{
			return await Context
				.Members
				.FirstOrDefaultAsync(x=>x.Name == name)
				.ConfigureAwait(false);
		}
	}
}
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PekoBot.Database.Repositories.Interfaces;
using PekoBot.Entities.Models;

namespace PekoBot.Database.Repositories
{
	public class MemberRepository : Repository<Member>, IMemberRepository
	{
		public MemberRepository(PekoBotContext context) : base(context)
		{
		}

		public async Task<Member> GetByChannelIdAsync(string channelId)
		{
			return await Context
				.Members
				.FirstOrDefaultAsync(x => x.YoutubeId == channelId)
				.ConfigureAwait(false);
		}
	}
}
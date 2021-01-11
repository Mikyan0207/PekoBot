using System;
using System.Threading.Tasks;
using PekoBot.Database.Repositories;
using PekoBot.Database.Repositories.Interfaces;

namespace PekoBot.Database
{
	public sealed class UnitOfWork : IUnitOfWork
	{
		public UnitOfWork(PekoBotContext context)
		{
			Context = context;
		}

		public PekoBotContext Context { get; set; }

		public int SaveChanges()
		{
			return Context.SaveChanges();
		}

		public async Task<int> SaveChangesAsync()
		{
			return await Context.SaveChangesAsync().ConfigureAwait(false);
		}

		public void Dispose()
		{
			Context.Dispose();
		}

		private IChannelRepository _channelRepository;
		public IChannelRepository Channels => _channelRepository ??= new ChannelRepository(Context);

		private ILiveRepository _liveRepository;
		public ILiveRepository Lives => _liveRepository ??= new LiveRepository(Context);

		private IMembersRepository _memberRepository;
		public IMembersRepository Members => _memberRepository ??= new MembersRepository(Context);

		private IRolesRepository _rolesRepository;
		public IRolesRepository Roles => _rolesRepository ??= new RolesRepository(Context);
	}
}
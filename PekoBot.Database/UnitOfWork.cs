using PekoBot.Database.Repositories;
using PekoBot.Database.Repositories.Interfaces;
using System.Threading.Tasks;

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

		public IRepository<T> AsRepository<T>() where T : class
		{
			return new Repository<T>(Context);
		}

		public void Dispose()
		{
			Context.Dispose();
		}

		private IChannelRepository _channelRepository;
		public IChannelRepository Channels => _channelRepository ??= new ChannelRepository(Context);

		private ICompanyRepository _companyRepository;
		public ICompanyRepository Companies => _companyRepository ??= new CompanyRepository(Context);

		private ILiveRepository _liveRepository;
		public ILiveRepository Lives => _liveRepository ??= new LiveRepository(Context);

		private IVTubersRepository _vtuberRepository;
		public IVTubersRepository VTubers => _vtuberRepository ??= new VTubersRepository(Context);

		private IMessageRepository _messageRepository;
		public IMessageRepository Messages => _messageRepository ??= new MessageRepository(Context);

		private IRolesRepository _rolesRepository;
		public IRolesRepository Roles => _rolesRepository ??= new RolesRepository(Context);
	}
}
using PekoBot.Database.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace PekoBot.Database
{
	public interface IUnitOfWork : IDisposable
	{
		PekoBotContext Context { get; set; }

		public IChannelRepository Channels { get; }

		public ICompanyRepository Companies { get; }

		public IGuildRepository Guilds { get; }

		public ILiveRepository Lives { get; }

		public IVTubersRepository VTubers { get; }

		public IMessageRepository Messages { get; }

		public IRolesRepository Roles { get; }

		int SaveChanges();

		Task<int> SaveChangesAsync();

		public IRepository<T> AsRepository<T>() where T : class;
	}
}
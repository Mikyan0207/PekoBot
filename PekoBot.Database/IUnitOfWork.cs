using System;
using System.Threading.Tasks;
using PekoBot.Database.Repositories.Interfaces;

namespace PekoBot.Database
{
	public interface IUnitOfWork : IDisposable
	{
		PekoBotContext Context { get; set; }

		public IChannelRepository Channels { get; }

		public ILiveRepository Lives { get; }

		public IMembersRepository Members { get; }

		public IMessageRepository Messages { get; }

		public IRolesRepository Roles { get; }

		int SaveChanges();

		Task<int> SaveChangesAsync();
	}
}
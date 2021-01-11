using System;
using System.Threading.Tasks;
using PekoBot.Database.Repositories.Interfaces;

namespace PekoBot.Database
{
	public interface IUnitOfWork : IDisposable
	{
		PekoBotContext Context { get; set; }

		public ILiveRepository Lives { get; }

		public IMemberRepository Members { get; }

		int SaveChanges();

		Task<int> SaveChangesAsync();
	}
}
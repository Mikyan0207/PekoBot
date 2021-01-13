using Microsoft.EntityFrameworkCore;
using NLog;
using PekoBot.Database.Repositories.Interfaces;
using PekoBot.Entities.Models;
using System.Threading.Tasks;

namespace PekoBot.Database.Repositories
{
	public class CompanyRepository : Repository<Company>, ICompanyRepository
	{
		private static Logger Logger { get; set; }

		public CompanyRepository(PekoBotContext context) : base(context)
		{
			Logger = LogManager.GetCurrentClassLogger();
		}

		public async Task<Company> GetOrCreate(string name)
		{
			var company = await Context.Companies.FirstOrDefaultAsync(x => x.Name == name).ConfigureAwait(false);

			if (company != null)
				return company;

			try
			{
				var e = await Context.AddAsync(new Company
				{
					Name = name
				}).ConfigureAwait(false);
				await Context.SaveChangesAsync().ConfigureAwait(false);

				return e.Entity;
			}
			catch (DbUpdateConcurrencyException ex)
			{
				Logger.Error(ex);
				return null;
			}
		}
	}
}
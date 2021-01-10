using Microsoft.EntityFrameworkCore;
using PekoBot.Entities;
using PekoBot.Entities.Models;

namespace PekoBot.Database
{
	public class PekoBotContext : DbContext
	{
		public DbSet<DiscordChannel> Channels { get; set; }

		public DbSet<Member> Members { get; set; }

		public DbSet<Live> Lives { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlite("Filename=PekoBot.db", options =>
			{
				options
					.CommandTimeout(60)
					.MigrationsAssembly("PekoBot.Database");
			});
		}

    }
}
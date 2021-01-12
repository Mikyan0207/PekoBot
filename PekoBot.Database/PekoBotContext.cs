using System;
using Microsoft.EntityFrameworkCore;
using PekoBot.Entities;
using PekoBot.Entities.Models;

namespace PekoBot.Database
{
	public class PekoBotContext : DbContext
	{
		public DbSet<Channel> Channels { get; set; }

		public DbSet<Company> Companies { get; set; }

		public DbSet<Guild> Guilds { get; set; }

		public DbSet<Member> Members { get; set; }

		public DbSet<Live> Lives { get; set; }

		public DbSet<Role> Roles { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlite("Filename=PekoBot.db", options =>
			{
				options
					.CommandTimeout(60)
					.MigrationsAssembly("PekoBot.Database");
			});
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Guild>()
				.HasMany(x => x.Channels)
				.WithOne(x => x.Guild);

			modelBuilder.Entity<Guild>()
				.HasMany(x => x.Roles)
				.WithOne(x => x.Guild);

			modelBuilder.Entity<Guild>()
				.HasMany(x => x.Users)
				.WithOne(x => x.Guild);

			modelBuilder.Entity<Member>()
				.HasMany(x => x.Roles)
				.WithOne(x => x.Member);

			modelBuilder.Entity<Company>()
				.HasMany(x => x.Members)
				.WithOne(x => x.Company);

			modelBuilder.Entity<Live>()
				.HasOne(x => x.Member);

			base.OnModelCreating(modelBuilder);
		}
	}
}
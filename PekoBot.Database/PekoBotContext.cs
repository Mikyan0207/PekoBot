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
			modelBuilder.Entity<Member>()
				.Property(x => x.Nicknames)
				.HasConversion(
					v => string.Join(',', v),
					v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));

			modelBuilder.Entity<Role>()
				.HasOne(x => x.Member)
				.WithOne(x => x.Role);

			base.OnModelCreating(modelBuilder);
		}
	}
}
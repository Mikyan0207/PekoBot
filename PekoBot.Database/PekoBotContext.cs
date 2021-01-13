using Microsoft.EntityFrameworkCore;
using PekoBot.Entities.Models;

namespace PekoBot.Database
{
	public class PekoBotContext : DbContext
	{
		public DbSet<Channel> Channels { get; set; }

		public DbSet<Company> Companies { get; set; }

		public DbSet<Emoji> Emojis { get; set; }

		public DbSet<Guild> Guilds { get; set; }

		public DbSet<Live> Lives { get; set; }

		public DbSet<Message> Messages { get; set; }

		public DbSet<Role> Roles { get; set; }

		public DbSet<Statistics> Statistics { get; set; }

		public DbSet<User> Users { get; set; }

		public DbSet<VTuber> VTubers { get; set; }

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
			modelBuilder.Entity<Company>()
				.HasMany(x => x.VTubers)
				.WithOne(x => x.Company);

			modelBuilder.Entity<Emoji>()
				.HasOne(x => x.VTuber)
				.WithOne(x => x.Emoji);

			modelBuilder.Entity<Guild>()
				.HasMany(x => x.Channels)
				.WithOne(x => x.Guild);

			modelBuilder.Entity<Guild>()
				.HasMany(x => x.Roles)
				.WithOne(x => x.Guild);

			modelBuilder.Entity<Guild>()
				.HasMany(x => x.Users)
				.WithOne(x => x.Guild);

			modelBuilder.Entity<Live>()
				.HasOne(x => x.VTuber);

			modelBuilder.Entity<VTuber>()
				.HasMany(x => x.Roles)
				.WithOne(x => x.VTuber);

			modelBuilder.Entity<Message>()
				.HasOne(x => x.Channel)
				.WithMany(x => x.Messages);

			modelBuilder.Entity<Message>()
				.HasOne(x => x.Author)
				.WithMany(x => x.Messages);

			base.OnModelCreating(modelBuilder);
		}
	}
}
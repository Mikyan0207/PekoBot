using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NLog;
using PekoBot.Core.Services.Interfaces;
using PekoBot.Database;
using PekoBot.Entities.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PekoBot.Core.Services
{
	public class DbService : IService
	{
		private Logger Logger { get; }

		public DbService()
		{
			Logger = LogManager.GetCurrentClassLogger();

			using var context = new PekoBotContext();
			if (context.Database.GetPendingMigrations().Any())
			{
				context.Database.Migrate();
				context.SaveChanges();
			}

			context.Database.ExecuteSqlRaw("PRAGMA journal_mode=WAL; PRAGMA synchronous=OFF");
			context.SaveChanges();
			Initialize(context);
			context.SaveChanges();
		}

		public PekoBotContext GetContext()
		{
			var context = new PekoBotContext();
			context.Database.SetCommandTimeout(60);

			var conn = context.Database.GetDbConnection();
			conn.Open();

			using var com = conn.CreateCommand();
			com.CommandText = "PRAGMA journal_mode=WAL; PRAGMA synchronous=OFF";
			com.ExecuteNonQuery();

			return context;
		}

		public IUnitOfWork GetUnitOfWork()
		{
			return new UnitOfWork(GetContext());
		}

		private void Initialize(PekoBotContext context)
		{
		}
	}
}
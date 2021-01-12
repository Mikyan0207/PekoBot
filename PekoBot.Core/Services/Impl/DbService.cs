using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NLog;
using PekoBot.Database;
using PekoBot.Entities;
using PekoBot.Entities.Enums;
using PekoBot.Entities.Models;

namespace PekoBot.Core.Services.Impl
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
			var content = File.ReadAllText($"Resources/Companies.json");
			var companies = JsonConvert.DeserializeObject<List<string>>(content);

			foreach (var company in companies.Where(company => !context.Companies.Any(x => x.Name == company)))
			{
				try
				{
					context.Companies.Add(new Company
					{
						Name = company
					});
					context.SaveChanges();
				}
				catch (DbUpdateConcurrencyException e)
				{
					Logger.Error(e);
				}
			}


			content = File.ReadAllText($"Resources/Members.json");
			var members = JsonConvert.DeserializeObject<List<Entities.Json.Member>>(content);

			foreach (var member in members.Where(member => !context.Members.Any(x => x.Name == member.Name)))
			{
				try
				{
					context.Members.Add(new Member
					{
						Name = member.Name,
						AvatarUrl = member.AvatarUrl,
						Company = context.Companies.FirstOrDefault(x => x.Name == member.Company),
						YoutubeId = member.YouTubeId
					});

					context.SaveChanges();
				}
				catch (DbUpdateConcurrencyException e)
				{
					Logger.Error(e);
				}
			}
		}
	}
}
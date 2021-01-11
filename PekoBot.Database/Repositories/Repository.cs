using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PekoBot.Database.Repositories.Interfaces;

namespace PekoBot.Database.Repositories
{
	public class Repository<T> : IRepository<T> where T : class
	{
		protected readonly PekoBotContext Context;
		protected DbSet<T> Set;

		public Repository(PekoBotContext context)
		{
			Context = context;
			Set = Context.Set<T>();
		}

		public T Get(string id)
		{
			return Set.Find(id);
		}

		public T Get(int id)
		{
			return Set.Find(id);
		}

		public T Get(ulong id)
		{
			return Set.Find(id);
		}

		public async Task<T> GetAsync(string id)
		{
			return await Set.FindAsync(id).ConfigureAwait(false);
		}

		public async Task<T> GetAsync(int id)
		{
			return await Set.FindAsync(id).ConfigureAwait(false);
		}

		public async Task<T> GetAsync(ulong id)
		{
			return await Set.FindAsync(id).ConfigureAwait(false);
		}

		public IEnumerable<T> GetAll()
		{
			return Set.ToList();
		}

		public void Add(T entity)
		{
			Set.Add(entity);
		}

		public async Task AddAsync(T entity)
		{
			await Set.AddAsync(entity).ConfigureAwait(false);
		}

		public void AddRange(IEnumerable<T> entities)
		{
			Set.AddRange(entities);
		}

		public async Task AddRangeAsync(IEnumerable<T> entities)
		{
			await Set.AddRangeAsync(entities).ConfigureAwait(false);
		}

		public void Remove(T entity)
		{
			Set.Remove(entity);
		}

		public void RemoveRange(IEnumerable<T> entities)
		{
			Set.RemoveRange(entities);
		}

		public void Update(T entity)
		{
			Set.Update(entity);
		}
	}
}
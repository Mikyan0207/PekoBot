﻿using System;
using Microsoft.EntityFrameworkCore;
using PekoBot.Database.Repositories.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

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

		public async Task<T> GetAsync(string id)
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

		public async Task<T> AddAsync(T entity)
		{
			var e = await Set.AddAsync(entity).ConfigureAwait(false);

			return e.Entity;
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

		public bool Any(Expression<Func<T, bool>> predicate)
		{
			return Set.Any(predicate);
		}

		public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
		{
			return await Set.AnyAsync(predicate).ConfigureAwait(false);
		}

		public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
		{
			return Set.Where(predicate);
		}

		public IQueryable<T> Where(Expression<Func<T, int, bool>> predicate)
		{
			return Set.Where(predicate);
		}
	}
}
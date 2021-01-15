using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PekoBot.Database.Repositories.Interfaces
{
	public interface IRepository<T> where T : class
	{
		T Get(string id);

		Task<T> GetAsync(string id);

		IEnumerable<T> GetAll();

		void Add(T entity);

		Task<T> AddAsync(T entity);

		void AddRange(IEnumerable<T> entities);

		Task AddRangeAsync(IEnumerable<T> entities);

		void Remove(T entity);

		void RemoveRange(IEnumerable<T> entities);

		void Update(T entity);

		bool Any(Expression<Func<T, bool>> predicate);

		Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

		IQueryable<T> Where(Expression<Func<T, bool>> predicate);

		IQueryable<T> Where(Expression<Func<T, int, bool>> predicate);
	}
}
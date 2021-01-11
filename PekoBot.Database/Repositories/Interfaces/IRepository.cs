using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PekoBot.Database.Repositories.Interfaces
{
	public interface IRepository<T> where T : class
	{
		T Get(string id);

		T Get(int id);

		T Get(ulong id);

		Task<T> GetAsync(string id);

		Task<T> GetAsync(int id);

		Task<T> GetAsync(ulong id);

		IEnumerable<T> GetAll();

		void Add(T entity);

		Task AddAsync(T entity);

		void AddRange(IEnumerable<T> entities);

		Task AddRangeAsync(IEnumerable<T> entities);

		void Remove(T entity);

		void RemoveRange(IEnumerable<T> entities);

		void Update(T entity);
	}
}
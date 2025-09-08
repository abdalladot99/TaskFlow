using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Core.Enitites;
using TaskFlow.Core.Interfaces;
using TaskFlow.Infrastructure.Data;

namespace TaskFlow.Infrastructure.Repositories
{
	public class Repository<T>: IRepository<T> where T : class
	{
		private readonly AppDbContext _dbContext;
		private readonly DbSet<T> _dbSet;

		public Repository(AppDbContext dbContext)
		{
			_dbContext = dbContext;
			_dbSet = _dbContext.Set<T>();
		}
 
		public async Task AddAsync(T entity)
		{
			await _dbSet.AddAsync(entity);
			await SaveChangesAsync();
		}
 
		public async Task<bool> DeleteAsync(string id)
		{
			var entity = await _dbSet.FindAsync(id);
			if (entity == null)
				return false;

			_dbSet.Remove(entity);
			await SaveChangesAsync();
			return true;
		}
 
		public async Task<List<T>> GetAllAsync()
		{
			var listEntity = await _dbSet.ToListAsync();
			return listEntity ?? new List<T>();
		}
		 
		public async Task<T?> GetByIdAsync(string id)
		{
			return await _dbSet.FindAsync(id);
		}
		 

		public async Task SaveChangesAsync()
		{
			await _dbContext.SaveChangesAsync();
		}
		 

		public async Task<T?> UpdateAsync(string id, T entity)
		{
			var foundEntity = await _dbSet.FindAsync(id);
			if (foundEntity == null)
				return null;

			_dbContext.Entry(foundEntity).CurrentValues.SetValues(entity);

			await SaveChangesAsync();
			return foundEntity;
		}

		public IQueryable<T> QueryableAsync()
		{
			return _dbSet.AsQueryable();
		}

		public T? GetByName(string name)
		{
		 
			var prop = typeof(T).GetProperty("Name");
			if (prop == null) return null;

			return _dbSet.AsEnumerable().FirstOrDefault(e => prop.GetValue(e)?.ToString() == name);
		}
	}
}

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
	public class PriorityRepository:IRepository<Priority>
	{
		private readonly AppDbContext _dbContext;

		public PriorityRepository(AppDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task AddAsync(Priority entity)
		{
			await _dbContext.Priorities.AddAsync(entity);
			await SaveChangesAsync();
		}

		public async Task<bool> DeleteAsync(string id)
		{
			var entity = await _dbContext.Priorities.FindAsync(id);
			if (entity == null)
				return false;
			_dbContext.Priorities.Remove(entity);
			await SaveChangesAsync();
			return true;
		}

		public async Task<List<Priority>> GetAllAsync()
		{
			var listEntity = await _dbContext.Priorities.ToListAsync();
			if (listEntity == null)
				return new List<Priority>();
			return listEntity;
		}

		public async Task<Priority> GetByIdAsync(string id)
		{
			var entity = await _dbContext.Priorities.FindAsync(id);
			if (entity == null)
				return new Priority();
			return entity;
		}
		 

		public async Task SaveChangesAsync()
		{
			await _dbContext.SaveChangesAsync();
		}

		public async Task<Priority> UpdateAsync(string id, Priority entity)
		{
			var foundEntity = await GetByIdAsync(id);
			if (foundEntity == null)
				return new Priority();

			foundEntity.Name = entity.Name;

			_dbContext.Priorities.Update(foundEntity);
			await SaveChangesAsync();
			return foundEntity;
		}

		public IQueryable<Priority> QueryableAsync()
		{
			return _dbContext.Priorities.AsQueryable();
		}
		 

		public Priority GetByName(string Name)
		{
			var entity = _dbContext.Priorities.FirstOrDefault(N => N.Name == Name);
			if (entity == null)
				return new Priority();
			return entity;
		}
	}
}

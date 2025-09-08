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
	public class StatusRepository:IRepository<Status>
	{
		private readonly AppDbContext _dbContext;

		public StatusRepository(AppDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task AddAsync(Status entity)
		{
			await _dbContext.Statuses.AddAsync(entity);
			await SaveChangesAsync();
		}

		public async Task<bool> DeleteAsync(string id)
		{
			var entity = await _dbContext.Statuses.FindAsync(id);
			if (entity == null)
				return false;
			_dbContext.Statuses.Remove(entity);
			await SaveChangesAsync();
			return true;
		}

		public async Task<List<Status>> GetAllAsync()
		{
			var listEntity = await _dbContext.Statuses.ToListAsync();
			if (listEntity == null)
				return new List<Status>();
			return listEntity;
		}

		public async Task<Status> GetByIdAsync(string id)
		{
			var entity = await _dbContext.Statuses.FindAsync(id);
			if (entity == null)
				return new Status();
			return entity;
		}


		public async Task SaveChangesAsync()
		{
			await _dbContext.SaveChangesAsync();
		}

		public async Task<Status> UpdateAsync(string id, Status entity)
		{
			var foundEntity = await GetByIdAsync(id);
			if (foundEntity == null)
				return new Status();

			foundEntity.Name = entity.Name;

			_dbContext.Statuses.Update(foundEntity);
			await SaveChangesAsync();
			return foundEntity;
		}

		public IQueryable<Status> QueryableAsync()
		{
			return _dbContext.Statuses.AsQueryable();
		}
		 
		public Status GetByName(string Name)
		{
			var entity = _dbContext.Statuses.FirstOrDefault(N => N.Name==Name);
			if (entity == null)
				return new Status();
			return entity;
		}
	}
}

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
	public class TaskRepository : IRepository<AppTask>
	{
		private readonly AppDbContext _dbContext;

		public TaskRepository(AppDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task AddAsync(AppTask entity)
		{
		   await _dbContext.Tasks.AddAsync(entity);
		   await  SaveChangesAsync();
		}

		public async Task<bool> DeleteAsync(string id)
		{
			var entity =await _dbContext.Tasks.FindAsync(id);
			if (entity == null) 
				return false;
			_dbContext.Tasks.Remove(entity);
			await SaveChangesAsync();
			return true;
		}

		public async Task<List<AppTask>> GetAllAsync()
		{
			var listEntity = await _dbContext.Tasks.ToListAsync();
			if (listEntity == null)
				return new List<AppTask>();
			return listEntity;
		}

		public async Task<AppTask> GetByIdAsync(string id)
		{
			var entity = await _dbContext.Tasks.FindAsync(id);
			if(entity == null)
				return new AppTask();
			return entity;
		}

		public AppTask GetByName(string Name)
		{
			var entity = _dbContext.Tasks.FirstOrDefault(N => N.Title == Name);
			if (entity == null)
				return new AppTask();
			return entity; 
		}

		public IQueryable<AppTask> QueryableAsync()
		{
			return _dbContext.Tasks.AsQueryable();	
		}

		public async Task SaveChangesAsync()
		{
			await _dbContext.SaveChangesAsync();
		}

		public async Task<AppTask> UpdateAsync(string id ,AppTask entity)
		{
			var foundEntity = await GetByIdAsync(id);
			if (foundEntity == null)
				return new AppTask();
			_dbContext.Tasks.Update(entity);
			await SaveChangesAsync();
			return foundEntity;
		}
		 
	}
}

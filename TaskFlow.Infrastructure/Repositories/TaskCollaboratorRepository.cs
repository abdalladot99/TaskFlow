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
	public class TaskCollaboratorRepository : ITaskCollaborator
	{
		private readonly AppDbContext _dbContext;
		private readonly DbSet<TaskCollaborator> _dbSet;

		public TaskCollaboratorRepository(AppDbContext dbContext)
		{
			_dbContext = dbContext;
			_dbSet = _dbContext.Set<TaskCollaborator>();
		}

		//public async Task<bool> DeleteAsyncc(TaskCollaborator entity)
		//{
		//	_dbSet.Remove(entity);
		//	await SaveChangesAsync();
		//	return true;
		//}


		public async Task UpdateAsync(TaskCollaborator collaborator)
		{
			_dbSet.Update(collaborator);
			await SaveChangesAsync();
		}


		public async Task<TaskCollaborator> GetByTaskAndUserIdAsync(string taskId, string userId)
		{
			return await _dbSet
				.FirstOrDefaultAsync(tc => tc.TaskId == taskId && tc.UserId == userId);
		}

		public async Task DeleteAsync(TaskCollaborator collaborator)
		{
			 _dbSet.Remove(collaborator);
			await SaveChangesAsync();
		}
		public IQueryable<TaskCollaborator> QueryableAsync()
		{
			return _dbSet.AsQueryable();
		}

		public async Task SaveChangesAsync()
		{
			await _dbContext.SaveChangesAsync();
		}


	}
}

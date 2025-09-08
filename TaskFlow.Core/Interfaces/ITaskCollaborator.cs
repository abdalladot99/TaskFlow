using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Core.Enitites;

namespace TaskFlow.Core.Interfaces
{
	public interface ITaskCollaborator
	{
		Task<TaskCollaborator> GetByTaskAndUserIdAsync(string taskId, string userId);
		Task DeleteAsync(TaskCollaborator collaborator); 
		Task SaveChangesAsync();
		IQueryable<TaskCollaborator> QueryableAsync();
		Task UpdateAsync(TaskCollaborator collaborator);


	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Core.Enitites;

namespace TaskFlow.Core.Interfaces
{
	// Generic Repository Interface
	public interface IRepository<T> where T : class
	{
		Task AddAsync(T entity);
		Task<T> UpdateAsync(string id, T entity);
		Task<bool> DeleteAsync(string id);
		Task<T> GetByIdAsync(string id);
		Task<List<T>> GetAllAsync();
		Task SaveChangesAsync();
		T GetByName(string Name);
		IQueryable<T> QueryableAsync();


	}

}

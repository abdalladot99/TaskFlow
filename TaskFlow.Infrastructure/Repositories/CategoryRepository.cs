using Microsoft.EntityFrameworkCore;
using TaskFlow.Core.Enitites;
using TaskFlow.Core.Interfaces;
using TaskFlow.Infrastructure.Data;

namespace TaskFlow.Infrastructure.Repositories
{
	public class CategoryRepository : IRepository<Category>
	{
		private readonly AppDbContext _dbContext;

		public CategoryRepository(AppDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task AddAsync(Category entity)
		{
			await _dbContext.Categories.AddAsync(entity);
			await SaveChangesAsync();
		}

		public async Task<bool> DeleteAsync(string id)
		{
			var entity = await _dbContext.Categories.FindAsync(id);
			if (entity == null)
				return false;
			_dbContext.Categories.Remove(entity);
			await SaveChangesAsync();
			return true;
		}

		public async Task<List<Category>> GetAllAsync()
		{
			var listEntity = await _dbContext.Categories.ToListAsync();
			if (listEntity == null)
				return new List<Category>();
			return listEntity;
		}

		public async Task<Category> GetByIdAsync(string id)
		{
			var entity = await _dbContext.Categories.FindAsync(id);
			if (entity == null)
				return new Category();
			return entity;
		}
		 
		public Category GetByName(string Name)
		{
			var entity = _dbContext.Categories.FirstOrDefault(N => N.Name == Name);
			if (entity == null)
				return new Category();
			return entity;
		}

		public async Task SaveChangesAsync()
		{
			await _dbContext.SaveChangesAsync();
		}

		public async Task<Category> UpdateAsync(string id, Category entity)
		{
			var foundEntity = await GetByIdAsync(id);
			if (foundEntity == null)
				return new Category();

			foundEntity.Name = entity.Name;
			 
			_dbContext.Categories.Update(foundEntity);
			await SaveChangesAsync();
			return foundEntity;
		}
		
		public IQueryable<Category> QueryableAsync() 
		{
			return _dbContext.Categories.AsQueryable();
		}
		 
	}
}

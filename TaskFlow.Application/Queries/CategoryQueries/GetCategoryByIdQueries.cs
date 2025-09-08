using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.DTOs.CategoryDTO;
using TaskFlow.Application.DTOs.TaskDTO;
using TaskFlow.Core.Enitites;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.Application.Queries.CategoryQueries
{
 
	public record GetCategoryByIdQueries(string id) : IRequest<DataCategoryDTO?>;


	public class GetCategoryByIdQueriesHandler(IRepository<Category> _CategoryRepository,IMapper _mapper)
		: IRequestHandler<GetCategoryByIdQueries, DataCategoryDTO?>
	{
		public async Task<DataCategoryDTO?> Handle(GetCategoryByIdQueries request, CancellationToken cancellationToken)
		{
			var queryCategory = await _CategoryRepository.QueryableAsync()
				.Include(t => t.Tasks)
				.FirstOrDefaultAsync(i => i.Id == request.id, cancellationToken);

			if (queryCategory == null)
				return null;
			 
			var listDataTasks = _mapper.Map<List<DataTaskDTO>>(queryCategory.Tasks);
			var listDataCategory = _mapper.Map<DataCategoryDTO>(queryCategory);

			listDataCategory.Tasks = listDataTasks;

			return listDataCategory; 
		}
	}
}

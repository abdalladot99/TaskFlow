using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.DTOs.CategoryDTO;
using TaskFlow.Application.DTOs.TaskDTO;
using TaskFlow.Core.Enitites;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.Application.Queries.CategoryQueries
{
	public record GetAllCategoryCurrentUserQueries(string userId): IRequest<IEnumerable<DataCategoryDTO>>;

	public class GetAllCategoryCurrentUserHandler(IRepository<Category> _categoryRepository,IMapper _mapper)
		: IRequestHandler<GetAllCategoryCurrentUserQueries, IEnumerable<DataCategoryDTO>>
	{
		public async Task<IEnumerable<DataCategoryDTO>> Handle(GetAllCategoryCurrentUserQueries request, CancellationToken cancellationToken)
		{ 
			var categories =await _categoryRepository.QueryableAsync()
				.Select(s=>new Category 
				{
					Id=s.Id,
					Name=s.Name,
					Tasks=s.Tasks.Where(t=>t.UserId==request.userId).ToList()
				})
 				.Where(p => p.Tasks.Any(t => t.UserId == request.userId))
				.ToListAsync(cancellationToken);

			if (categories == null || categories.Count == 0)
			{
				return Enumerable.Empty<DataCategoryDTO>();
			}
			List<DataCategoryDTO> listDataCategory = new List<DataCategoryDTO>();

			foreach (var item in categories)
			{
				var listDataTasks = _mapper.Map<List<DataTaskDTO>>(item.Tasks);
				var listDataCategoryDTO = _mapper.Map<DataCategoryDTO>(item);
				listDataCategoryDTO.Tasks = listDataTasks;

				listDataCategory.Add(listDataCategoryDTO);
			}
			return listDataCategory;
		}
	}
}

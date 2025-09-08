using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.DTOs.CategoryDTO;
using TaskFlow.Application.DTOs.TaskDTO;
using TaskFlow.Core.Enitites;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.Application.Queries.CategoryQueries
{
	public record GetCategoryByIdForUserQuery(string categoryId, string userId):IRequest<DataCategoryDTO>;


	public class GetCategoryByIdForUserHandler(IRepository<Category> _categoryRepository, IMapper _mapper)
		: IRequestHandler<GetCategoryByIdForUserQuery, DataCategoryDTO>
	{
		public async Task<DataCategoryDTO> Handle(GetCategoryByIdForUserQuery request, CancellationToken cancellationToken)
		{
			var categories = await _categoryRepository.QueryableAsync()
				.Select(s => new Category
				{
					Id = s.Id,
					Name = s.Name,
					Tasks = s.Tasks.Where(t => t.UserId == request.userId).ToList()
				})
 				.Where(p => p.Tasks.Any(t => t.UserId == request.userId))
				.FirstOrDefaultAsync(i=>i.Id==request.categoryId,cancellationToken);

			if (categories == null)
				return new DataCategoryDTO();

				var listDataTasks = _mapper.Map<List<DataTaskDTO>>(categories.Tasks);
				var listDataCategoryDTO = _mapper.Map<DataCategoryDTO>(categories);
				listDataCategoryDTO.Tasks = listDataTasks;

 			return listDataCategoryDTO;
		}
	}



}

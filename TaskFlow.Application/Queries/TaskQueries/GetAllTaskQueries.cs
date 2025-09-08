using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.DTOs.TaskDTO;
using TaskFlow.Core.Enitites;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.Application.Queries.TaskQueries
{
	 
	public record GetAllTaskQueries() : IRequest<List<BigDataViewTaskDTO>>;


	public class GetAllTaskQueriesHandler(IRepository<AppTask> taskRepository,IMapper _mapper)
		: IRequestHandler<GetAllTaskQueries, List<BigDataViewTaskDTO>>
	{
		public async Task<List<BigDataViewTaskDTO>> Handle(GetAllTaskQueries request , CancellationToken cancellationToken)
		{
			var listTasks = await taskRepository.QueryableAsync()
 				.Include(i => i.Category)
				.Include(i => i.Priority)
				.Include(i => i.Status)
				.Include(i=>i.User)
				.Include(i => i.RecurrenceType)
				.ToListAsync(cancellationToken);

			var entity = _mapper.Map<List<BigDataViewTaskDTO>>(listTasks);

			return entity;
 
		}
	}
}

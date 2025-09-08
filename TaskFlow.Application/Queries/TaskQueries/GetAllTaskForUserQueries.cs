using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.DTOs.TaskDTO;
using TaskFlow.Core.Enitites;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.Application.Queries.TaskQueries
{
 
	public record GetAllTaskForUserQueries(string userId) : IRequest<List<BigDataViewTaskDTO>>;


	public class GetAllTaskForUserHandler(IRepository<AppTask> _taskRepository ,IMapper _mapper)
		: IRequestHandler<GetAllTaskForUserQueries, List<BigDataViewTaskDTO>>
	{
		public async Task<List<BigDataViewTaskDTO>> Handle(GetAllTaskForUserQueries request, CancellationToken cancellationToken)
		{
			var listTasks = await _taskRepository.QueryableAsync()
				.Where(i => i.UserId == request.userId)
				.Include(i => i.Category)
				.Include(i => i.Priority)
				.Include(i=>i.User)
				.Include(i => i.Status)
				.Include(i => i.RecurrenceType)
				.ToListAsync(cancellationToken);

			var entity = _mapper.Map<List<BigDataViewTaskDTO>>(listTasks);
 
			return entity;

		}
	}
}

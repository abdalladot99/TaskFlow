using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.DTOs.TaskDTO;
using TaskFlow.Core.Enitites;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.Application.Queries.Task
{
	public record GetTaskByIdQueries(string id) : IRequest<BigDataViewTaskDTO>;


	public class GetTaskByIdQueriesHandler  (IRepository<AppTask> _taskRepository, IMapper _mapper)
		: IRequestHandler<GetTaskByIdQueries, BigDataViewTaskDTO>
	{
		 
		public async Task<BigDataViewTaskDTO> Handle(GetTaskByIdQueries request, CancellationToken cancellationToken)
		{
			var foundTask = await _taskRepository.QueryableAsync()
				.Include(c => c.Category)
				.Include(c => c.Collaborators)
				.Include(c => c.Notifications)
				.Include(c => c.Priority)
				.Include(c => c.RecurrenceType)
				.Include(c => c.Status)
				.Include(c => c.User)
				.Include(c => c.Collaborators)
				.FirstOrDefaultAsync(t => t.Id == request.id, cancellationToken);
 
			var entity = _mapper.Map<BigDataViewTaskDTO>(foundTask);
			 
			return entity;



		}
	}


}

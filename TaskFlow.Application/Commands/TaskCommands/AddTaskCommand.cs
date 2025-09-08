using AutoMapper;
using MediatR;
using TaskFlow.Application.DTOs.TaskDTO;
using TaskFlow.Core.Enitites;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.Application.Commands.TaskCommands
{

	public record AddTaskCommand(string userId,AddTaskDTO Task) : IRequest<AppTask>;

	public class AddTaskHandler(IRepository<AppTask> _taskRepository, IMapper _mapper)
		: IRequestHandler<AddTaskCommand, AppTask>
	{ 
		public async Task<AppTask> Handle(AddTaskCommand command, CancellationToken cancellationToken)
		{
		 
 			var entity = _mapper.Map<AppTask>(command.Task);
			entity.UserId = command.userId;
			entity.CreatedAt = DateTime.UtcNow;
			entity.LastUpdatedAt = DateTime.UtcNow;
			entity.CompletedAt = null; 

			await _taskRepository.AddAsync(entity);

 			return entity;
		}
	}



}

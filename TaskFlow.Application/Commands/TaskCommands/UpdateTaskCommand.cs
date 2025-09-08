using System.Globalization;
using MediatR;
using TaskFlow.Application.DTOs.TaskDTO;
using TaskFlow.Core.Enitites;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.Application.Commands.TaskCommands
{
	public record UpdateTaskCommand(UpdateTaskDTO Dto): IRequest<(bool,AppTask)>;

	public class UpdateTaskHandler(IRepository<AppTask> _taskRepository): IRequestHandler<UpdateTaskCommand, (bool, AppTask)>
	{
	 
		public async Task<(bool, AppTask)> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
		{
			var entity =await _taskRepository.GetByIdAsync(request.Dto.TaskId);
			if (entity == null)
				return (false,new AppTask());

			if (!string.IsNullOrWhiteSpace(request.Dto.Title))
				entity.Title = request.Dto.Title;


			if (!string.IsNullOrWhiteSpace(request.Dto.Description))
				entity.Description = request.Dto.Description;

			if(!string.IsNullOrWhiteSpace(request.Dto.CategoryId))
				entity.CategoryId = request.Dto.CategoryId;

			if(!string.IsNullOrWhiteSpace(request.Dto.PriorityId))
				entity.PriorityId = request.Dto.PriorityId;

			if(!string.IsNullOrWhiteSpace(request.Dto.StatusId))
				entity.StatusId = request.Dto.StatusId; 
			 
			if(!string.IsNullOrWhiteSpace(request.Dto.RecurrTypeId))
				entity.RecurrenceTypeId = request.Dto.RecurrTypeId;
			 
 			entity.LastUpdatedAt = DateTime.UtcNow;

			var newEntity = await _taskRepository.UpdateAsync(request.Dto.TaskId,entity);

			return (true,newEntity);
		}
	}


}

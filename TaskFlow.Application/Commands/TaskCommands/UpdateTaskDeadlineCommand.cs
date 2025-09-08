using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TaskFlow.Core.Enitites;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.Application.Commands.TaskCommands
{
	public sealed record UpdateTaskDeadlineCommand(string TaskId, DateTime NewDueDate, string? ActorUserId = null)
	: IRequest<bool>;

	public sealed class UpdateTaskDeadlineHandler(IRepository<AppTask> _taskRepository) : IRequestHandler<UpdateTaskDeadlineCommand, bool>
	{
		public async Task<bool> Handle(UpdateTaskDeadlineCommand request, CancellationToken cancellationToken)
		{
			var task = await _taskRepository.GetByIdAsync(request.TaskId);
			if (task is null) 
				return false;
			 
			task.DueDate = request.NewDueDate;
			task.LastUpdatedAt = DateTime.UtcNow;

			await _taskRepository.UpdateAsync(task.Id,task); 
			return true;
		}
	}
}

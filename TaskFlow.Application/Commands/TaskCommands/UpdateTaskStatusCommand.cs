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
	public record UpdateTaskStatusCommand(string TaskId, string StatusId) : IRequest<bool>;

	public class UpdateTaskStatusHandler : IRequestHandler<UpdateTaskStatusCommand, bool>
	{
		private readonly IRepository<AppTask> _taskRepository;

		public UpdateTaskStatusHandler(IRepository<AppTask> taskRepository)
		{ 
			_taskRepository = taskRepository;
		}

		public async Task<bool> Handle(UpdateTaskStatusCommand request, CancellationToken cancellationToken)
		{
			var task = await _taskRepository.GetByIdAsync(request.TaskId);
			if (task == null)
				return false;

			task.StatusId = request.StatusId;
			task.LastUpdatedAt = DateTime.UtcNow;
			if (request.StatusId == "Completed") // Assuming "Completed" is the ID for the completed status
			{
				task.CompletedAt = DateTime.UtcNow;
			}
			await _taskRepository.UpdateAsync(task.Id, task);
			return true;
		}
	}

}

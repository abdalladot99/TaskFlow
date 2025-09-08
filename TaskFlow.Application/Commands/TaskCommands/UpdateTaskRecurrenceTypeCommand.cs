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
	public record UpdateTaskRecurrenceTypeCommand(string TaskId, string RecurrenceTypeId) : IRequest<bool>;

	public class UpdateTaskRecurrenceTypeCommandHandler
	: IRequestHandler<UpdateTaskRecurrenceTypeCommand, bool>
	{
		private readonly IRepository<AppTask> _taskRepository;

		public UpdateTaskRecurrenceTypeCommandHandler(IRepository<AppTask> taskRepository)
		{
			_taskRepository = taskRepository;
		}

		public async Task<bool> Handle(UpdateTaskRecurrenceTypeCommand request, CancellationToken cancellationToken)
		{
			var task = await _taskRepository.GetByIdAsync(request.TaskId);
			if (task == null)
				return false;

			task.RecurrenceTypeId = request.RecurrenceTypeId;
			task.LastUpdatedAt = DateTime.UtcNow;

			await _taskRepository.UpdateAsync(task.Id, task);
			return true;
		}
	}



}

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
	public record UpdateTaskPriorityCommand(string TaskId, string PriorityId) : IRequest<bool>;

	public class UpdateTaskPriorityCommandHandler
		: IRequestHandler<UpdateTaskPriorityCommand, bool>
	{
		private readonly IRepository<AppTask> _taskRepository;

		public UpdateTaskPriorityCommandHandler(IRepository<AppTask> taskRepository)
		{
			_taskRepository = taskRepository;
		}

		public async Task<bool> Handle(UpdateTaskPriorityCommand request, CancellationToken cancellationToken)
		{
			var task = await _taskRepository.GetByIdAsync(request.TaskId);
			if (task == null) return false;

			task.PriorityId = request.PriorityId;

			await _taskRepository.UpdateAsync(task.Id, task);

			return true;
		}
	}

}

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
	public record UpdateTaskCategoryCommand(string TaskId, string CategoryId) : IRequest<bool>;
	 

	public class UpdateTaskCategoryCommandHandler
	: IRequestHandler<UpdateTaskCategoryCommand, bool>
	{
		private readonly IRepository<AppTask> _taskRepository;

		public UpdateTaskCategoryCommandHandler(IRepository<AppTask> taskRepository)
		{
			_taskRepository = taskRepository;
		}

		public async Task<bool> Handle(UpdateTaskCategoryCommand request, CancellationToken cancellationToken)
		{
			var task = await _taskRepository.GetByIdAsync(request.TaskId);
			if (task == null)
				return false;

			task.CategoryId = request.CategoryId;
			task.LastUpdatedAt = DateTime.UtcNow;

			await _taskRepository.UpdateAsync(task.Id,task);
			return true;
		}
	}


}

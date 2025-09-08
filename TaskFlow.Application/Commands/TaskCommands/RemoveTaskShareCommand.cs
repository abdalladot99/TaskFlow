using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Core.Enitites;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.Application.Commands.TaskCommands
{
	public record RemoveTaskShareCommand(string TaskId, string UserId, string CurrentUserId) : IRequest<bool>;

	public class RemoveTaskShareCommandHandler(ITaskCollaborator collaboratorRepository
		, IRepository<AppTask> taskRepository) 
		: IRequestHandler<RemoveTaskShareCommand, bool>
	{
		public async Task<bool> Handle(RemoveTaskShareCommand request, CancellationToken cancellationToken)
		{
 			var task = await taskRepository.GetByIdAsync(request.TaskId);
			if (task == null || task.UserId != request.CurrentUserId)
				return false;

 			var collaborator = await collaboratorRepository.QueryableAsync()
				.FirstOrDefaultAsync(c => c.TaskId == request.TaskId && c.UserId == request.UserId, cancellationToken);

			if (collaborator == null)
				return false;

			await collaboratorRepository.DeleteAsync(collaborator);

			return true;
		}
	}


}

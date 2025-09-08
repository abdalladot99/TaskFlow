using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.Application.Commands.TaskCommands
{
	public record RemoveTaskCollaboratorCommand(string TaskId, string UserId) : IRequest<bool>;


	public class RemoveTaskCollaboratorCommandHandler
	: IRequestHandler<RemoveTaskCollaboratorCommand, bool>
	{
		private readonly  ITaskCollaborator _taskCollaboratorRepository;

		public RemoveTaskCollaboratorCommandHandler(ITaskCollaborator taskCollaboratorRepository)
		{
			_taskCollaboratorRepository = taskCollaboratorRepository;
		}

		public async Task<bool> Handle(RemoveTaskCollaboratorCommand request, CancellationToken cancellationToken)
		{
			var collaborator = await _taskCollaboratorRepository
				.GetByTaskAndUserIdAsync(request.TaskId, request.UserId);

			if (collaborator == null)
				return false;

			await _taskCollaboratorRepository.DeleteAsync(collaborator);
			return true;
		}
	}


}

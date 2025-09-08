using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.Application.Commands.TaskCommands
{
	public record UpdateTaskCollaboratorStatusCommand(string TaskId, string UserId, bool IsAccepted) : IRequest<bool>;


	public class UpdateTaskCollaboratorStatusHandler
	: IRequestHandler<UpdateTaskCollaboratorStatusCommand, bool>
	{
		private readonly ITaskCollaborator _repository;

		public UpdateTaskCollaboratorStatusHandler(ITaskCollaborator repository)
		{
			_repository = repository;
		}

		public async Task<bool> Handle(UpdateTaskCollaboratorStatusCommand request, CancellationToken cancellationToken)
		{
			var collaborator = await _repository.GetByTaskAndUserIdAsync(request.TaskId, request.UserId);

			if (collaborator == null)
				return false;

			collaborator.IsAccepted = request.IsAccepted;

			await _repository.UpdateAsync(collaborator);
			return true;
		}
	}

}

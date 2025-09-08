using MediatR;
using TaskFlow.Core.Enitites;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.Application.Commands.TaskCommands
{
	public record DeleteTaskCommand(string TaskId) : IRequest<bool>;

	public class DeleteTaskCommandHandler(IRepository<AppTask> _taskRepository)
		: IRequestHandler<DeleteTaskCommand, bool>
	{
		public async Task<bool> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
		{ 
			return await _taskRepository.DeleteAsync(request.TaskId);

		}
	}

}

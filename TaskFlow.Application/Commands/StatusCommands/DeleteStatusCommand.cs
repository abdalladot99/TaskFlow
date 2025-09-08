using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TaskFlow.Core.Enitites;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.Application.Commands.StatusCommands
{ 
	public record DeleteStatusCommand(string Id) : IRequest<bool>;


	public class DeleteStatusHandler : IRequestHandler<DeleteStatusCommand, bool>
	{
		private readonly IRepository<Status> _statusRepository;

		public DeleteStatusHandler(IRepository<Status> StatusRepository)
		{
			_statusRepository = StatusRepository;
		}

		public async Task<bool> Handle(DeleteStatusCommand request, CancellationToken cancellationToken)
		{
			var priority = await _statusRepository.DeleteAsync(request.Id);

			return priority;
		}
	}
}

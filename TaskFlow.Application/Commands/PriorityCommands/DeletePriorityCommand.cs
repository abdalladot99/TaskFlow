using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TaskFlow.Core.Enitites;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.Application.Commands.PriorityCommands
{
	public record DeletePriorityCommand(string Id) : IRequest<bool>;


	public class DeletePriorityHandler : IRequestHandler<DeletePriorityCommand, bool>
	{
		private readonly IRepository<Priority> _priorityRepository;

		public DeletePriorityHandler(IRepository<Priority> priorityRepository)
		{
			_priorityRepository = priorityRepository;
		}

		public async Task<bool> Handle(DeletePriorityCommand request, CancellationToken cancellationToken)
		{
 			var priority = await _priorityRepository.DeleteAsync(request.Id);
			 
 			return priority;
		}
	}

}

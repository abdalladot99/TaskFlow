using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TaskFlow.Core.Enitites;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.Application.Commands.RecurrenceTypeCommands
{
	public record DeleteRecurrenceTypeCommand(string Id) : IRequest<bool>;


	public class DeleteRecurrenceTypeHandler : IRequestHandler<DeleteRecurrenceTypeCommand, bool>
	{
		private readonly IRepository<RecurrenceType> _recurrenceTypeRepository;

		public DeleteRecurrenceTypeHandler(IRepository<RecurrenceType> recurrenceTypeRepository)
		{
			_recurrenceTypeRepository = recurrenceTypeRepository;
		}

		public async Task<bool> Handle(DeleteRecurrenceTypeCommand request, CancellationToken cancellationToken)
		{
			var priority = await _recurrenceTypeRepository.DeleteAsync(request.Id);

			return priority;
		}
	}
}

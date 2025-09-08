using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TaskFlow.Application.DTOs.PriorityDto;
using TaskFlow.Core.Enitites;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.Application.Commands.PriorityCommands
{
	public record UpdatePriorityCommand(Priority oldentity,AddPriorityDto newentity) : IRequest<Priority>;

	public class UpdatePriorityHandler(IRepository<Priority> _priorityepository, IMapper _mapper)
	: IRequestHandler<UpdatePriorityCommand, Priority>
	{
		public async Task<Priority> Handle(UpdatePriorityCommand command, CancellationToken cancellationToken)
		{ 
			var entity = _mapper.Map<Priority>(command.newentity);

			return await _priorityepository.UpdateAsync(command.oldentity.Id,entity); 
		}
	}
}

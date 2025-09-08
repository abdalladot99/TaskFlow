using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TaskFlow.Application.DTOs.PriorityDto;
using TaskFlow.Application.DTOs.StatusDTO;
using TaskFlow.Core.Enitites;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.Application.Commands.StatusCommands
{

	public record AddStatusCommand(AddStatusDto Dto) : IRequest<Status>;

	public class AddStatusHandler(IRepository<Status> _statusRepository, IMapper _mapper)
		: IRequestHandler<AddStatusCommand, Status>
	{
		public async Task<Status> Handle(AddStatusCommand command, CancellationToken cancellationToken)
		{
			var founded = _statusRepository.GetByName(command.Dto.Name);
			if (founded.Name == null)
			{
				var entity = _mapper.Map<Status>(command.Dto); 
				var states = _statusRepository.AddAsync(entity);
				return entity;
			}
			return founded;
		}

	}
}
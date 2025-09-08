using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using AutoMapper;
using MediatR; 
using TaskFlow.Application.DTOs.PriorityDto;
using TaskFlow.Core.Enitites;
using TaskFlow.Core.Enum;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.Application.Commands.PriorityCommands
{
	public record AddPriorityCommand(AddPriorityDto Dto) :IRequest<Priority>;

	public class AddPriorityHandler(IRepository<Priority> _priorityRepository, IMapper _mapper)
		: IRequestHandler<AddPriorityCommand, Priority> 
	{
		public async Task<Priority> Handle(AddPriorityCommand command, CancellationToken cancellationToken)
		{
			var founded = _priorityRepository.GetByName(command.Dto.Name);
			if (founded.Name == null)
			{
				var entity = _mapper.Map<Priority>(command.Dto);

				entity.Level = entity.Name switch
				{
					"High" => 1,
					"Medium" => 2,
					"Low" => 3,
					_ => 99
				};

				var prioritya = _priorityRepository.AddAsync(entity);
				return entity;
			}
			return founded;
		}

	}
	 
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TaskFlow.Application.DTOs.PriorityDto;
using TaskFlow.Application.DTOs.RecurrenceTypeDTO;
using TaskFlow.Core.Enitites;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.Application.Commands.RecurrenceTypeCommands
{ 		 
	public record AddRecurrenceTypeCommand(AddRecurrenceTypeDto Dto) : IRequest<RecurrenceType>;

	public class AddRecurrenceTypeHandler(IRepository<RecurrenceType> _recurrenceTypeRepository, IMapper _mapper)
		: IRequestHandler<AddRecurrenceTypeCommand, RecurrenceType>
	{
		public async Task<RecurrenceType> Handle(AddRecurrenceTypeCommand command, CancellationToken cancellationToken)
		{
			var founded = _recurrenceTypeRepository.GetByName(command.Dto.Name);
			if (founded == null)
			{
				var entity = _mapper.Map<RecurrenceType>(command.Dto);

				entity.IntervalInDays = entity.Name switch
				{ 
					"Daily" => 1,
					"Weekly" => 7,
					"Monthly" => 30,
					"Yearly" => 365,
					_ => 1000
				};

				var result = _recurrenceTypeRepository.AddAsync(entity);

				return entity;
			}
			return founded;
		}

	}

}

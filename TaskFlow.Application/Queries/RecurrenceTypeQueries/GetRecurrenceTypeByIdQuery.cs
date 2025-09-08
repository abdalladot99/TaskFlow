using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.DTOs.RecurrenceTypeDTO;
using TaskFlow.Application.DTOs.TaskDTO;
using TaskFlow.Core.Enitites;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.Application.Queries.RecurrenceTypeQueries
{ 
	public record GetRecurrenceTypeByIdQuery(string Id) : IRequest<DataRecurrenceTypeDTO>;


	public class GetPriorityByIdQueryHandler : IRequestHandler<GetRecurrenceTypeByIdQuery,DataRecurrenceTypeDTO>
	{
		private readonly IRepository<RecurrenceType> _recurrenceTypeRepository;
		private readonly IMapper _mapper;

		public GetPriorityByIdQueryHandler(IRepository<RecurrenceType> recurrenceTypeRepository,IMapper mapper)
		{
			_recurrenceTypeRepository = recurrenceTypeRepository;
			_mapper = mapper;
		}

		public async Task<DataRecurrenceTypeDTO> Handle(GetRecurrenceTypeByIdQuery request, CancellationToken cancellationToken)
		{

			var recurrence = await _recurrenceTypeRepository.QueryableAsync()
				.Include(t => t.Tasks)
 				.FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

			if(recurrence==null)
				return new DataRecurrenceTypeDTO();

 
		    var dataTaskDTO = _mapper.Map<List<DataTaskDTO>>(recurrence.Tasks);
			var dataRecurrenceTypeDTO = _mapper.Map<DataRecurrenceTypeDTO>(recurrence);
			dataRecurrenceTypeDTO.Tasks = dataTaskDTO;

			return dataRecurrenceTypeDTO; 
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
	public record GetRecurrenceTypeByIdForUserQuery(string RecurrId, string UserId) : IRequest<DataRecurrenceTypeDTO>;



	public class GetRecurrenceTypeByIdForUserHandler(IRepository<RecurrenceType> _recurrenceTypeRepository, IMapper _mapper)
		: IRequestHandler<GetRecurrenceTypeByIdForUserQuery, DataRecurrenceTypeDTO>
	{
		public async Task<DataRecurrenceTypeDTO> Handle(GetRecurrenceTypeByIdForUserQuery request, CancellationToken cancellationToken)
		{


			var queryRecurrence = await _recurrenceTypeRepository.QueryableAsync()
				.Select(r => new DataRecurrenceTypeDTO
				{
					Id = r.Id,
					Name = r.Name,
					IntervalInDays = r.IntervalInDays,
					Tasks = r.Tasks
						.Where(t => t.UserId == request.UserId)
						.Select(t => new DataTaskDTO
						{
							Id = t.Id,
							Title = t.Title,
							Description = t.Description,
							DueDate = t.DueDate
						}).ToList()
				})
				.Where(r => r.Tasks.Any())
				.FirstOrDefaultAsync(i => i.Id == request.RecurrId,cancellationToken);

 			if (queryRecurrence == null) 
				return new DataRecurrenceTypeDTO();

 
			var listDataTasks = _mapper.Map<List<DataTaskDTO>>(queryRecurrence.Tasks);

			var listDataRecurrenceDTO = _mapper.Map<DataRecurrenceTypeDTO>(queryRecurrence);

			listDataRecurrenceDTO.Tasks = listDataTasks;
			 

			return listDataRecurrenceDTO;
		}
	}


}

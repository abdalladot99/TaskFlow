using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.DTOs.RecurrenceTypeDTO;
using TaskFlow.Application.DTOs.TaskDTO;
using TaskFlow.Core.Enitites;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.Application.Queries.RecurrenceTypeQueries
{
	public record GetAllRecurrenceTypeForUserQueries(string userId):IRequest<List<DataRecurrenceTypeDTO>>;

	public class GetAllRecurrenceTypeForUserQueriesHandler(IRepository<RecurrenceType> _recurrenceTypeRepository,IMapper _mapper)
		: IRequestHandler<GetAllRecurrenceTypeForUserQueries, List<DataRecurrenceTypeDTO>>
	{
		public async Task<List<DataRecurrenceTypeDTO>> Handle(GetAllRecurrenceTypeForUserQueries request, CancellationToken cancellationToken)
		{

		 
			var queryRecurrence = await _recurrenceTypeRepository.QueryableAsync()
				.Select(r => new DataRecurrenceTypeDTO
				{
					Id = r.Id,
					Name = r.Name,
					IntervalInDays = r.IntervalInDays,
					Tasks = r.Tasks
						.Where(t => t.UserId == request.userId)
						.Select(t => new DataTaskDTO
						{
							Id = t.Id,
							Title = t.Title,
							Description=t.Description,
							DueDate = t.DueDate
						}).ToList()
				})
				.Where(r => r.Tasks.Any())
				.ToListAsync(cancellationToken);


			List<DataRecurrenceTypeDTO> listDataRecurrence = new List<DataRecurrenceTypeDTO>();

			foreach (var item in queryRecurrence)
			{
				var listDataTasks = _mapper.Map<List<DataTaskDTO>>(item.Tasks);

				var listDataRecurrenceDTO = _mapper.Map<DataRecurrenceTypeDTO>(item);

				listDataRecurrenceDTO.Tasks = listDataTasks;

				listDataRecurrence.Add(listDataRecurrenceDTO);
			}

			return listDataRecurrence;
		}
	}

}

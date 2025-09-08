using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.DTOs.PriorityDto;
using TaskFlow.Application.DTOs.TaskDTO;
using TaskFlow.Core.Enitites;
using TaskFlow.Core.Interfaces;
 
namespace TaskFlow.Application.Queries.PriorityQueries
{
	public record GetAllPriorityForUserQueries(string userId) : IRequest<List<DataPriorityDTO>>;


	public class GetAllPriorityForUserHandler
		: IRequestHandler<GetAllPriorityForUserQueries, List<DataPriorityDTO>>
	{
		private readonly IRepository<Priority> _priorityRepository;
		private readonly IMapper _mapper;

		public GetAllPriorityForUserHandler(IRepository<Priority> priorityRepository, IMapper mapper)
		{
			_priorityRepository = priorityRepository;
			_mapper = mapper;
		}

		public async Task<List<DataPriorityDTO>> Handle(GetAllPriorityForUserQueries request, CancellationToken cancellationToken)
		{ 
			var queryPriority = await _priorityRepository.QueryableAsync()
				.Select(s=>new Priority 
				{
					Id=s.Id,
					Name=s.Name,
					Tasks=s.Tasks.Where(t=>t.UserId==request.userId).ToList()
				})
 				.Where(p => p.Tasks.Any(t => t.UserId == request.userId))
				.ToListAsync(cancellationToken);

			List<DataPriorityDTO> listDataPriority = new List<DataPriorityDTO>();

			foreach (var item in queryPriority)
			{
				var listDataTasks = _mapper.Map<List<DataTaskDTO>>(item.Tasks);
				var listDataPriorityDTO = _mapper.Map<DataPriorityDTO>(item);
				listDataPriorityDTO.Tasks = listDataTasks;

				listDataPriority.Add(listDataPriorityDTO);
			}
			return listDataPriority;
		}


	}


}

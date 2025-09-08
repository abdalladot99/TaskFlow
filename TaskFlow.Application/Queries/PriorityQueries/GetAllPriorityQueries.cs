using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.DTOs.PriorityDto;
using TaskFlow.Application.DTOs.TaskDTO;
using TaskFlow.Core.Enitites;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.Application.Queries.PriorityQueries
{
	public record GetAllPriorityQueries():IRequest<List<Priority>>;

	 
	public class GetAllPriorityHandler
		: IRequestHandler<GetAllPriorityQueries, List<Priority>>
	{
		private readonly IRepository<Priority> _priorityRepository;
		private readonly IMapper _mapper;

		public GetAllPriorityHandler(IRepository<Priority> priorityRepository,IMapper mapper)
		{
			_priorityRepository = priorityRepository;
			_mapper = mapper;
		}

		public async Task<List<Priority>> Handle(GetAllPriorityQueries request, CancellationToken cancellationToken)
		{
			var queryPriority = await _priorityRepository.GetAllAsync();
			 
			return queryPriority; 
		}


	}


}

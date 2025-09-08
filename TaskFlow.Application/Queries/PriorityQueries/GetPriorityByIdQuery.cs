using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.DTOs.PriorityDto;
using TaskFlow.Application.DTOs.TaskDTO;
using TaskFlow.Core.Enitites;
using TaskFlow.Core.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
 
namespace TaskFlow.Application.Queries.PriorityQueries
{
	public record GetPriorityByIdQuery(string Id) : IRequest<DataPriorityDTO?>;


	public class GetPriorityByIdQueryHandler : IRequestHandler<GetPriorityByIdQuery, DataPriorityDTO?>
	{
		private readonly IRepository<Priority> _priorityRepository;
		private readonly IMapper _mapper;

		public GetPriorityByIdQueryHandler(IRepository<Priority> priorityRepository, IMapper mapper)
		{
			_priorityRepository = priorityRepository;
			_mapper = mapper;
		}

		public async Task<DataPriorityDTO?> Handle(GetPriorityByIdQuery request, CancellationToken cancellationToken)
		{
			var queryPriority = await _priorityRepository.QueryableAsync()
				.Include(t => t.Tasks)
				.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

			if (queryPriority == null)
				return null;
			 
			var listDataTasks = _mapper.Map<List<DataTaskDTO>>(queryPriority.Tasks);
			var listDataPriority = _mapper.Map<DataPriorityDTO>(queryPriority);

			listDataPriority.Tasks = listDataTasks;

			return listDataPriority;

		}
	}

}

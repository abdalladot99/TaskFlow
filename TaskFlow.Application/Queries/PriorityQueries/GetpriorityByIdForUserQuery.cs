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

namespace TaskFlow.Application.Queries.PriorityQueries
{ 
	public record GetpriorityByIdForUserQuery(string priorityId,string userId) : IRequest<DataPriorityDTO>;


	public class GetpriorityByIdForUserHandler
		: IRequestHandler<GetpriorityByIdForUserQuery, DataPriorityDTO>
	{
		private readonly IRepository<Priority> _priorityRepository;
		private readonly IMapper _mapper;

		public GetpriorityByIdForUserHandler(IRepository<Priority> priorityRepository, IMapper mapper)
		{
			_priorityRepository = priorityRepository;
			_mapper = mapper;
		}

		public async Task<DataPriorityDTO> Handle(GetpriorityByIdForUserQuery request, CancellationToken cancellationToken)
		{
			var queryPriority = await _priorityRepository.QueryableAsync()
				.Select(s => new Priority
				{
					Id = s.Id,
					Name = s.Name,
					Tasks = s.Tasks.Where(t => t.UserId == request.userId).ToList()
				})
				.Where(p => p.Tasks.Any(t => t.UserId == request.userId))
				.FirstOrDefaultAsync(i => i.Id == request.priorityId, cancellationToken);
 

			if (queryPriority==null)
			{
				return new DataPriorityDTO();
			}
			var listDataTasks = _mapper.Map<List<DataTaskDTO>>(queryPriority.Tasks);
			var listDataPriorityDTO = _mapper.Map<DataPriorityDTO>(queryPriority);
			listDataPriorityDTO.Tasks = listDataTasks;

  			return listDataPriorityDTO;
		}


	}

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.DTOs.StatusDTO;
using TaskFlow.Application.DTOs.TaskDTO;
using TaskFlow.Core.Enitites;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.Application.Queries.StatusQueries
{
	public record GetStatusByIdForUserQuery(string StatusId, string UserId) : IRequest<DataStatusDTO>;

	public class GetStatusByIdForUserHandler(IRepository<Status> _statusRepository,IMapper _mapper) 
		: IRequestHandler<GetStatusByIdForUserQuery, DataStatusDTO>
	{ 
		public async Task<DataStatusDTO> Handle(GetStatusByIdForUserQuery request, CancellationToken cancellationToken)
		{

			var queryStatus = await _statusRepository.QueryableAsync()
				.Select(s => new Status
				{
					Id = s.Id,
					Name = s.Name,
					Tasks = s.Tasks.Where(t => t.UserId == request.UserId).ToList()
				})
				.Where(s => s.Tasks.Any())
 				.FirstOrDefaultAsync(i=>i.Id==request.StatusId,cancellationToken);

				if (queryStatus==null)
				{
					return new DataStatusDTO();
				}
				var listDataTask = _mapper.Map<List<DataTaskDTO>>(queryStatus.Tasks);
				var listDataStatusDTO = _mapper.Map<DataStatusDTO>(queryStatus);
				listDataStatusDTO.Tasks = listDataTask; 

			return listDataStatusDTO;
		}
	}

}

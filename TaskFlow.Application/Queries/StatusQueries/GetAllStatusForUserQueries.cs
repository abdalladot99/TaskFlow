using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.DTOs.StatusDTO;
using TaskFlow.Application.DTOs.TaskDTO;
using TaskFlow.Core.Enitites;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.Application.Queries.StatusQueries
{
	public record GetAllStatusForUserQueries(string userId):IRequest<List<DataStatusDTO>>;
	 
	public class GetAllStatusForUserHandler(IMapper _mapper, IRepository<Status> _statusRepository)
			: IRequestHandler<GetAllStatusForUserQueries, List<DataStatusDTO>>
	{

		public async Task<List<DataStatusDTO>> Handle(GetAllStatusForUserQueries request, CancellationToken cancellationToken)
		{
			var queryStatus = await _statusRepository.QueryableAsync()
				.Select(s => new Status
				{
					Id = s.Id,
					Name = s.Name,
					Tasks = s.Tasks.Where(t => t.UserId == request.userId).ToList()
				})
				.Where(s => s.Tasks.Any()) 
				.ToListAsync(cancellationToken);


			List<DataStatusDTO> listDataStatus = new List<DataStatusDTO>();

			foreach (var item in queryStatus)
			{
				var listDataTask = _mapper.Map<List<DataTaskDTO>>(item.Tasks);
				var listDataStatusDTO = _mapper.Map<DataStatusDTO>(item);
				listDataStatusDTO.Tasks = listDataTask;

				listDataStatus.Add(listDataStatusDTO);
			}

			return listDataStatus;
		}
	}
}

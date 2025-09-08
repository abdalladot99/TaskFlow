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
	public record GetStatusByIdQuery(string Id) : IRequest<DataStatusDTO>;


	public class GetStatusByIdHandler : IRequestHandler<GetStatusByIdQuery, DataStatusDTO>
	{
		private readonly IRepository<Status> _statusRepository;
		private readonly IMapper _mapper;

		public GetStatusByIdHandler(IRepository<Status> StatusRepository,IMapper mapper)
		{
			_statusRepository = StatusRepository;
			_mapper = mapper;
		}

		public async Task<DataStatusDTO> Handle(GetStatusByIdQuery request, CancellationToken cancellationToken)
		{
			var status = await _statusRepository.QueryableAsync()
				.Include(t => t.Tasks)
				.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

			if (status == null)
				return new DataStatusDTO();

			var listDataTask = _mapper.Map<List<DataTaskDTO>>(status.Tasks);
			var dataStatus = _mapper.Map<DataStatusDTO>(status);
			dataStatus.Tasks=listDataTask;

			return dataStatus;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.DTOs.TaskCollaboratiorDTO;
using TaskFlow.Core.Enitites;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.Application.Queries.TaskCollaboratorQueries
{
	public record GetTaskCollaboratorsQuery(string TaskId) : IRequest<List<CollaboratorDTO>>;

	public class GetTaskCollaboratorsHandler
	: IRequestHandler<GetTaskCollaboratorsQuery, List<CollaboratorDTO>>
	{
		private readonly IRepository<TaskCollaborator> _repository;
		private readonly IMapper _mapper;

		public GetTaskCollaboratorsHandler(IRepository<TaskCollaborator> repository, IMapper mapper)
		{
			_repository = repository;
			_mapper = mapper;
		}

		public async Task<List<CollaboratorDTO>> Handle(GetTaskCollaboratorsQuery request, CancellationToken cancellationToken)
		{
			var collaborators = await _repository.QueryableAsync()
				.Where(c => c.TaskId == request.TaskId)
				.Include(c => c.User)  
				.ToListAsync(cancellationToken);

			return _mapper.Map<List<CollaboratorDTO>>(collaborators);
		}
	}


}

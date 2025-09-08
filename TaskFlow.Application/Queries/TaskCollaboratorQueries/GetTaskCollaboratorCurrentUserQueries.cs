using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.DTOs.TaskDTO;
using TaskFlow.Core.Enitites;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.Application.Queries.TaskCollaboratorQueries
{
	public record GetTaskCollaboratorCurrentUserQueries(string userId):IRequest<List<ViewTaskCollaboratorsMeDTO>>;

	public class GetSharedTasksQueryHandler( IRepository<TaskCollaborator> taskRepository, IMapper mapper)
		: IRequestHandler<GetTaskCollaboratorCurrentUserQueries, List<ViewTaskCollaboratorsMeDTO>>
	{
		public async Task<List<ViewTaskCollaboratorsMeDTO>> Handle(GetTaskCollaboratorCurrentUserQueries request, CancellationToken cancellationToken)
		{
			var sharedTasks = await taskRepository.QueryableAsync()
				.Where(c => c.UserId == request.userId && c.IsAccepted)
				.Include(c => c.Task)
					.ThenInclude(t => t.Category)
				.Include(c => c.Task)
					.ThenInclude(t => t.Priority)
				.Include(c => c.Task)
					.ThenInclude(t => t.RecurrenceType)
				.Include(c => c.Task)
					.ThenInclude(t => t.Status)
				.Include(c => c.Task)
					.ThenInclude(t => t.User)
				.ToListAsync(cancellationToken);

			var viewTaskCollaboratorsMeDTO = new List<ViewTaskCollaboratorsMeDTO>();

			if (!sharedTasks.Any())
			{
				return viewTaskCollaboratorsMeDTO;
			}

			foreach (var item in sharedTasks)
			{
				var taskDto = mapper.Map<BigDataViewTaskDTO>(item.Task);
				var listViewData = mapper.Map<ViewTaskCollaboratorsMeDTO>(item);
				listViewData.Tasks = new List<BigDataViewTaskDTO> { taskDto };

				viewTaskCollaboratorsMeDTO.Add(listViewData);
			}

			return viewTaskCollaboratorsMeDTO;
		}

	}
}

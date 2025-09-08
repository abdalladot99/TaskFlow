using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.DTOs.TaskDTO;
using TaskFlow.Core.Enitites;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.Application.Queries.TaskQueries
{
	public record GetUpcomingTasksQuery(string UserId) : IRequest<List<TaskDto>>;


	public class GetUpcomingTasksHandler
	: IRequestHandler<GetUpcomingTasksQuery, List<TaskDto>>
	{
		private readonly IRepository<AppTask> _taskRepository;

		public GetUpcomingTasksHandler(IRepository<AppTask> taskRepository)
		{
			_taskRepository = taskRepository;
		}

		public async Task<List<TaskDto>> Handle(GetUpcomingTasksQuery request, CancellationToken cancellationToken)
		{
			var startOfWeek = DateTime.UtcNow.Date;
			var endOfWeek = startOfWeek.AddDays(7);
			var tasks = await _taskRepository.QueryableAsync()
				.Where(t => t.UserId == request.UserId &&
							t.DueDate >= startOfWeek &&
							t.DueDate <= endOfWeek)

				.Select(t => new TaskDto
				{
					Id = t.Id,
					Title = t.Title,
					Description = t.Description,
					DueDate = t.DueDate,
					StatusId = t.StatusId
				})
				.ToListAsync(cancellationToken);

			return tasks;
		}
	}

}

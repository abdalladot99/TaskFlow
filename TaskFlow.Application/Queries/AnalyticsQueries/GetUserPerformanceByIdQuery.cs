using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.DTOs.AnalyticsDTO;
using TaskFlow.Core.Enitites;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.Application.Queries.AnalyticsQueries
{
	public record GetUserPerformanceByIdQuery(string UserId, DateTime? From, DateTime? To)
	: IRequest<UserPerformanceDTO>;


	public class GetUserPerformanceByIdHandler
	: IRequestHandler<GetUserPerformanceByIdQuery, UserPerformanceDTO>
	{
		private readonly IRepository<AppTask> _taskRepository;

		public GetUserPerformanceByIdHandler(IRepository<AppTask> taskRepository)
		{
			_taskRepository = taskRepository;
		}

		public async Task<UserPerformanceDTO> Handle(GetUserPerformanceByIdQuery request, CancellationToken cancellationToken)
		{
			var query = _taskRepository.QueryableAsync()
				.Include(t => t.User)
				.Where(t => t.UserId == request.UserId);

			if (request.From.HasValue)
				query = query.Where(t => t.DueDate >= request.From.Value);

			if (request.To.HasValue)
				query = query.Where(t => t.DueDate <= request.To.Value);

			var result = await query
				.GroupBy(t => new { t.UserId, t.User.UserName })
				.Select(g => new UserPerformanceDTO
				{
					UserId = g.Key.UserId,
					UserName = g.Key.UserName,
					CompletedTasks = g.Count(t => t.Status.Name == "Completed"),
					DelayedTasks = g.Count(t => t.Status.Name == "Delayed"),
					InProgressTasks = g.Count(t => t.Status.Name == "In Progress")
				})
				.FirstOrDefaultAsync(cancellationToken);

			return result ?? new UserPerformanceDTO
			{
				UserId = request.UserId,
				UserName = "N/A",
				CompletedTasks = 0,
				DelayedTasks = 0,
				InProgressTasks = 0
			};
		}
	}

}

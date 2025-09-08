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
	public record GetUserPerformanceQuery(DateTime? From, DateTime? To): IRequest<List<UserPerformanceDTO>>;

	public class GetUserPerformanceHandler
	: IRequestHandler<GetUserPerformanceQuery, List<UserPerformanceDTO>>
	{
		private readonly IRepository<AppTask> _taskRepository;

		public GetUserPerformanceHandler(IRepository<AppTask> taskRepository)
		{
			_taskRepository = taskRepository;
		}

		public async Task<List<UserPerformanceDTO>> Handle(GetUserPerformanceQuery request, CancellationToken cancellationToken)
		{
			var query = _taskRepository.QueryableAsync()
				.Include(t => t.User)
				.AsQueryable();

			if (request.From.HasValue)
				query = query.Where(t => t.DueDate >= request.From.Value);

			if (request.To.HasValue)
				query = query.Where(t => t.DueDate <= request.To.Value);

			var stats = await query
				.GroupBy(t => new { t.UserId, t.User.UserName })
				.Select(g => new UserPerformanceDTO
				{
					UserId = g.Key.UserId,
					UserName = g.Key.UserName,
					CompletedTasks = g.Count(t => t.Status.Name == "Completed"),
					DelayedTasks = g.Count(t => t.Status.Name == "Delayed"),
					InProgressTasks = g.Count(t => t.Status.Name == "In Progress")
				})
				.ToListAsync(cancellationToken);

			return stats;
		}
	}


}

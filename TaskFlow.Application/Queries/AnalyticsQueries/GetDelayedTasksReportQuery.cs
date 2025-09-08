using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Core.Enitites;
using TaskFlow.Core.Enum;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.Application.Queries.AnalyticsQueries
{
	public record GetDelayedTasksReportQuery(string UserId, DateTime From, DateTime To) : IRequest<int>;

	public class GetDelayedTasksReportHandler(IRepository<AppTask> _taskRepository, IRepository<Status> _statusRepository)
	: IRequestHandler<GetDelayedTasksReportQuery, int>
	{

		public async Task<int> Handle(GetDelayedTasksReportQuery request, CancellationToken cancellationToken)
		{
			var status = _statusRepository.GetByName(StatusTaskEnum.Completed.ToString());

			var count = await _taskRepository.QueryableAsync()
				.Where(t => t.UserId == request.UserId &&
							t.DueDate >= request.From &&
							t.DueDate <= request.To &&
							t.DueDate < DateTime.UtcNow && // متأخرة
							t.StatusId != status.Id) // ولسه متعملتش
				.ToListAsync(cancellationToken);

			return count.Count;
		}
	}
}

using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Core.Enitites;
using TaskFlow.Core.Enum;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.Application.Queries.AnalyticsQueries
{
	public record GetCompletedTasksReportQuery(string UserId, DateTime From, DateTime To) : IRequest<int>;

	public class Getcompletedtasksreporthandler(IRepository<AppTask> _taskrepository, IRepository<Status> _statusRepository)
		: IRequestHandler<GetCompletedTasksReportQuery, int>
	{

		public async Task<int> Handle(GetCompletedTasksReportQuery request, CancellationToken cancellationtoken)
		{
			var status = _statusRepository.GetByName(StatusTaskEnum.Completed.ToString());

			var count = await _taskrepository.QueryableAsync()
				.Where(t => t.UserId == request.UserId &&
							t.StatusId == status.Id &&
							t.CompletedAt >= request.From &&
							t.CompletedAt <= request.To)
				.CountAsync(cancellationtoken);

			return count;
		}
	}
}

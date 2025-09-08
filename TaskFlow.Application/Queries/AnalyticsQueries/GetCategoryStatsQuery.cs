using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.DTOs.AnalyticsDTO;
using TaskFlow.Core.Enitites;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.Application.Queries.AnalyticsQueries
{
	public record GetCategoryStatsQuery(DateTime? From, DateTime? To) : IRequest<List<CategoryStatsDTO>>;

	public class GetCategoryStatsHandler
	: IRequestHandler<GetCategoryStatsQuery, List<CategoryStatsDTO>>
	{
		private readonly IRepository<AppTask> _taskRepository;

		public GetCategoryStatsHandler(IRepository<AppTask> taskRepository)
		{
			_taskRepository = taskRepository;
		}

		public async Task<List<CategoryStatsDTO>> Handle(GetCategoryStatsQuery request, CancellationToken cancellationToken)
		{
			var query = _taskRepository.QueryableAsync()
				.Include(t => t.Category)
				.AsQueryable();

			if (request.From.HasValue)
				query = query.Where(t => t.DueDate >= request.From.Value);

			if (request.To.HasValue)
				query = query.Where(t => t.DueDate <= request.To.Value);

			var stats = await query
				.GroupBy(t => t.Category.Name)
				.Select(g => new CategoryStatsDTO
				{
					CategoryName = g.Key,
					TaskCount = g.Count()
				})
				.ToListAsync(cancellationToken);

			return stats;
		}
	}
}

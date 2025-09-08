using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Commands.TaskCommands;
using TaskFlow.Application.Queries.AnalyticsQueries;
using TaskFlow.Application.Queries.CategoryQueries;

namespace TaskFlow.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class AnalyticsController : ControllerBase
	{
		private readonly ISender _mediator;
		public AnalyticsController(ISender mediator)
		{
			_mediator = mediator;
		}

		 
		 
 		// عدد المهام المكتملة خلال فترة
		// GET api/reports/completed-tasks?from=2025-09-01&to=2025-09-07
		[HttpGet("completed-tasks")]
		public async Task<IActionResult> GetCompletedTasks([FromQuery] DateTime from, [FromQuery] DateTime to)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (userId == null)
				return Unauthorized();

			var result = await _mediator.Send(new GetCompletedTasksReportQuery(userId, from, to));

			return Ok(result);
		}
		 




		// عدد المهام المتأخرة خلال فترة
		// GET api/reports/delayed-tasks?from=2025-09-01&to=2025-09-07
		[HttpGet("delayed-tasks")]
		public async Task<IActionResult> GetDelayedTasks([FromQuery] DateTime from, [FromQuery] DateTime to)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (userId == null)
				return Unauthorized();

			var result = await _mediator.Send(new GetDelayedTasksReportQuery(userId, from, to));

			return Ok(result);
		}



 		// إحصائيات حسب التصنيفات
		// GET api/reports/category-stats
		[HttpGet("category-stats")]
		public async Task<IActionResult> GetCategoryStats([FromQuery] DateTime? from, [FromQuery] DateTime? to)
		{
			var result = await _mediator.Send(new GetCategoryStatsQuery(from, to));
			return Ok(result);
		}



 		// أداء المستخدمين
		// GET api/reports/user-performance
		[HttpGet("users-performance")]
		public async Task<IActionResult> GetUserPerformance([FromQuery] DateTime? from, [FromQuery] DateTime? to)
		{
			var result = await _mediator.Send(new GetUserPerformanceQuery(from, to));
			return Ok(result);
		}



		//اداء المستخدم الحالي
		// GET api/reports/my-performance
		[HttpGet("my-performance")]
		public async Task<IActionResult> GetMyPerformance([FromQuery] DateTime? from, [FromQuery] DateTime? to)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (userId == null)
				return Unauthorized();

			var result = await _mediator.Send(new GetUserPerformanceByIdQuery(userId, from, to));
			return Ok(result);
		}



	}
}

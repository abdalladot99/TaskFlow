using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Commands.PriorityCommands;
using TaskFlow.Application.Commands.StatusCommands;
using TaskFlow.Application.DTOs.PriorityDto;
using TaskFlow.Application.DTOs.StatusDTO;
using TaskFlow.Application.Queries.PriorityQueries;
using TaskFlow.Application.Queries.StatusQueries;
using TaskFlow.Core.Enitites;
using TaskFlow.Core.Enum;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]

	public class StatusController : ControllerBase
	{
		private readonly ISender _mediator;
		private readonly IRepository<Status> _statusrepository;

		public StatusController(ISender mediator, IRepository<Status> statusrepository)
		{
			_mediator = mediator;
			_statusrepository = statusrepository;
		}


		// GET: api/status
		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var result = await _mediator.Send(new GetAllStatusQueries());
			return Ok(result);
		}


		// GET: api/status
		[HttpGet("current-user")]
		public async Task<IActionResult> GetForUserAll()
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (userId == null)
				return Unauthorized();
			var result = await _mediator.Send(new GetAllStatusForUserQueries(userId));
			return Ok(result);
		}



		//put / add-status
		[HttpPost]
		public async Task<IActionResult> AddStatus([FromBody] AddStatusDto dto)
		{
			if (!Enum.TryParse<StatusTaskEnum>(dto.Name, true, out var StatusName))
				return BadRequest("Invalid status name. Allowed: Pending, InProgress, Completed");

 			var result = await _mediator.Send(new AddStatusCommand(dto));

			if (result is null)
				return BadRequest("Unexpected error occurred.");

			return Ok(result);
		}




		// DELETE: api/status/{id}
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteStatus(string id)
		{
			var result = await _mediator.Send(new DeleteStatusCommand(id));

			if (!result)
				return NotFound(new { Message = "Status not found or could not be deleted" });

			return Ok(new { Message = "Done, Status deleted successfully" });
		}


		// GetById: api/Status/{id}
		[HttpGet("{id}")]
		public async Task<IActionResult> GetStatusById(string id)
		{
			var result = await _mediator.Send(new GetStatusByIdQuery(id));

			if (result is null)
				return NotFound(new { Message = "Status not found" });

			return Ok(result);
		}


 		// GET: api/Status/current/{id}
		[HttpGet("{id}/current-user")]
		public async Task<IActionResult> GetStatusByIdForCurrentUser(string id)
		{
 			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(userId))
				return Unauthorized(new { Message = "User not authenticated" });

 			var result = await _mediator.Send(new GetStatusByIdForUserQuery(id, userId));

			if (result is null)
				return NotFound(new { Message = "Status not found for this user" });

			return Ok(result);
		}





	}
}

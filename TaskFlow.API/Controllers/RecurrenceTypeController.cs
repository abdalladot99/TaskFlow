using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Commands.RecurrenceTypeCommands;
using TaskFlow.Application.DTOs.RecurrenceTypeDTO;
using TaskFlow.Application.Queries.RecurrenceTypeQueries;
using TaskFlow.Application.Queries.StatusQueries;
using TaskFlow.Core.Enum;

namespace TaskFlow.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]

	public class RecurrenceTypeController : ControllerBase
	{
		private readonly ISender _mediator;
 
		public RecurrenceTypeController(ISender mediator)
		{
			_mediator = mediator;
 		}


		// GET: api/RecurrenceType
		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var result = await _mediator.Send(new GetAllRecurrenceTypeQueries());
			return Ok(result);
		}


		// GET: api/RecurrenceTypeForUser

		[HttpGet("current-user")]
		public async Task<IActionResult> GetForCurrentUser()
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (userId == null)
				return Unauthorized();

			var result = await _mediator.Send(new GetAllRecurrenceTypeForUserQueries(userId));
			return Ok(result);
		}



		//put / addRecurrenceType
		[HttpPost]
		public async Task<IActionResult> AddRecurrenceType([FromBody] AddRecurrenceTypeDto dto)
		{
			if (!Enum.TryParse<RecurrenceTypeNameEnum>(dto.Name, true, out var RecurrenceTypeName))
				return BadRequest("Invalid recurrenceType name. Allowed: Daily, Weekly, Monthly, Yearly");
		 
			var result = await _mediator.Send(new AddRecurrenceTypeCommand(dto));

			if (result is null)
				return BadRequest("Unexpected error occurred.");

			return Ok(result);
		}


		// DELETE: api/recurrenceType/{id}
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteRecurrenceType(string id)
		{
			var result = await _mediator.Send(new DeleteRecurrenceTypeCommand(id));

			if (!result)
				return NotFound(new { Message = "RecurrenceType not found or could not be deleted" });

			return Ok(new { Message = "Done, RecurrenceType deleted successfully" });
		}



		// GetById: api/RecurrenceType/{id}
		[HttpGet("{id}")]
		public async Task<IActionResult> GetRecurrenceTypeById(string id)
		{
			var result = await _mediator.Send(new GetRecurrenceTypeByIdQuery(id));

			if (result is null)
				return NotFound(new { Message = "RecurrenceType not found" });

			return Ok(result);//return recurrence include list<task> for everyone
		}



		// GET: api/RecurrenceType/current/{id}
		[HttpGet("{id}/current-user")]
		public async Task<IActionResult> GetRecurrenceTypeByIdForCurrentUser(string id)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(userId))
				return Unauthorized(new { Message = "User not authenticated" });

			var result = await _mediator.Send(new GetRecurrenceTypeByIdForUserQuery(id, userId));

			if (result is null)
				return NotFound(new { Message = "RecurrenceType not found for this user" });

			return Ok(result);
		}



		
	}
}

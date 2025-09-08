using System.Security.Claims;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.Commands.CategoryCommands;
using TaskFlow.Application.Commands.PriorityCommands;
using TaskFlow.Application.DTOs.Category;
using TaskFlow.Application.DTOs.PriorityDto;
using TaskFlow.Application.Queries.PriorityQueries;
using TaskFlow.Application.Queries.RecurrenceTypeQueries;
using TaskFlow.Core.Enitites;
using TaskFlow.Core.Enum;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]

	public class PriorityController : ControllerBase
	{
		private readonly ISender _mediator;
		private readonly IRepository<Priority> _priorityrepository;

		public PriorityController(ISender mediator,IRepository<Priority> priorityrepository)
		{
			_mediator = mediator;
			_priorityrepository = priorityrepository;
		}


		// GET: api/priority
		[HttpGet]
		public async Task<IActionResult> GetAll()
		{ 
			var result = await _mediator.Send(new GetAllPriorityQueries());
			return Ok(result);
		}


		// GET: api/current-user-priority
		[HttpGet("current-user")]
		public async Task<IActionResult> GetAllCurrentUser()
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (userId == null)
				return Unauthorized();
			var result = await _mediator.Send(new GetAllPriorityForUserQueries(userId));
			return Ok(result);
		}


		//put / addPriority
		[HttpPost]
		public async Task<IActionResult> AddPriority([FromBody] AddPriorityDto dto)
		{
			if (!Enum.TryParse<PriorityNameEnum>(dto.Name, true, out var priorityName))
				return BadRequest("Invalid priority name. Allowed: High, Medium, Low");

			var result = await _mediator.Send(new AddPriorityCommand(dto));  
			  
			if (result is  null)
				return BadRequest("Unexpected error occurred.");

			return Ok(result); 
		}
 
			 

		// PUT: api/priority/updatePriority/{id}
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdatePriority(string id, [FromBody] AddPriorityDto dto)
		{ 
			var priority =await _priorityrepository.GetByIdAsync(id);
			if (priority == null)
				return NotFound();

			if (!Enum.TryParse<PriorityNameEnum>(dto.Name, true, out var priorityName))
				return BadRequest("Invalid priority name. Allowed: High, Medium, Low");

			var result = await _mediator.Send(new UpdatePriorityCommand(priority,dto));
			  
			return Ok(result);
		}

		 
		// DELETE: api/priority/{id}
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeletePriority(string id)
		{
			var result = await _mediator.Send(new DeletePriorityCommand(id));

			if (!result)
				return NotFound(new { Message = "Priority not found or could not be deleted" });

			return Ok(new { Message = "Done, Priority deleted successfully" });
		}


		// GetById: api/priority/{id}
		[HttpGet("{id}")]
		public async Task<IActionResult> GetPriorityById(string id)
		{ 
			var result = await _mediator.Send(new GetPriorityByIdQuery(id));

			if (result is null)
				return NotFound(new { Message = "Priority not found" });

			return Ok(result);
		}




		// GET: api/priority/current/{id}
		[HttpGet("{id}/current-user")]
		public async Task<IActionResult> GetpriorityByIdForCurrentUser(string id)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(userId))
				return Unauthorized(new { Message = "User not authenticated" });

			var result = await _mediator.Send(new GetpriorityByIdForUserQuery(id, userId));

			if (result is null)
				return NotFound(new { Message = "priority not found for this user" });

			return Ok(result);
		}




	}
}

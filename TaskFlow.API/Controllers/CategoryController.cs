using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Commands.Categorycommands;
using TaskFlow.Application.Commands.CategoryCommands;
using TaskFlow.Application.DTOs.Category;
using TaskFlow.Application.Queries.CategoryQueries;
namespace TaskFlow.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class CategoryController : ControllerBase
	{ 

		private readonly ISender _mediator;

		public CategoryController(ISender mediator)
		{
			_mediator = mediator;
		}


		[HttpPost]
		public async Task<IActionResult> AddCategory([FromBody] AddCategoryDTO task)
		{
			var result = await _mediator.Send(new AddCategoryCommand(task));
			if (result == null)
			{
				return BadRequest("Task could not be added.");
			}
			   return Ok(result);
		}


		[HttpGet]
		public async Task<IActionResult> GetAllCategory()
		{
			var result = await _mediator.Send(new GetAllCategoryQueries());
			if (result == null)
			{
				return BadRequest();
			}
			return Ok(result);
		}


		[HttpGet("current-user")]
		public async Task<IActionResult> GetAllCategoryCurrentUser()
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (userId == null)
				return Unauthorized();
			var result = await _mediator.Send(new GetAllCategoryCurrentUserQueries(userId));
			if (result == null)
			{
				return BadRequest();
			}
			return Ok(result);
		}


		// GET: api/Category/current/{id}
		[HttpGet("{id}/current-user")]
		public async Task<IActionResult> GetCategoryByIdForCurrentUser(string id)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(userId))
				return Unauthorized(new { Message = "User not authenticated" });

			var result = await _mediator.Send(new GetCategoryByIdForUserQuery(id, userId));

			if (result is null)
				return NotFound(new { Message = "Category not found for this user" });

			return Ok(result);
		}




		[HttpGet("{id}")]
		public async Task<IActionResult> GetCategoryById([FromRoute] string id)
		{
			var result = await _mediator.Send(new GetCategoryByIdQueries(id));
			if (result == null)
			{
				return BadRequest();
			}
			return Ok(result); 
		}


		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateCategory([FromRoute] string id, [FromBody] EditCategoryDTO entityNewDTO)
		{
			var result = await _mediator.Send(new UpdateCategoryCommand(id,entityNewDTO));
			if (result == null)
			{
				return BadRequest();
			}
			return Ok(result);
		
		}


		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCategory([FromRoute] string id)
		{
			var result = await _mediator.Send(new DeleteCategoryCommand(id));
			return Ok(result);
		
		}



	}
}

using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Commands.TaskCommands;
using TaskFlow.Application.DTOs.TaskCollaboratiorDTO;
using TaskFlow.Application.DTOs.TaskDTO;
using TaskFlow.Application.Queries.Task;
using TaskFlow.Application.Queries.TaskCollaboratorQueries;
using TaskFlow.Application.Queries.TaskQueries;

namespace TaskFlow.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class AppTaskController : ControllerBase
	{

		private readonly ISender _mediator;
		public AppTaskController(ISender mediator)
		{
			_mediator = mediator;
		} 


		[HttpPost("Add-Task")]
		public async Task<IActionResult> AddTask([FromBody] AddTaskDTO task) 
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			 
			if (string.IsNullOrEmpty(userId))
				return Unauthorized();

 
			var result = await _mediator.Send(new AddTaskCommand(userId,task));
			if (result == null)
			{
				return BadRequest("Task could not be added.");
			}
			return Ok(result);
		}



		[HttpPut]
		public async Task<IActionResult> UpdateTask([FromBody] UpdateTaskDTO task) 
		{
			var result = await _mediator.Send(new UpdateTaskCommand(task));
			if (!result.Item1)
			{
				return BadRequest(new {Massage = "Task could not be updated.",success=false });
			}
			return Ok(new
			{
				success = true,
				TaskId = result.Item2.Id,
				massage = "Task updated successfully"
			});
		}



		[HttpGet]
		public async Task<IActionResult> GetAllTask() 
		{
			var result = await _mediator.Send(new GetAllTaskQueries());
			if (result == null)
			{
				return BadRequest();
			}
			return Ok(result);
		}


		[HttpGet("current-user")]
		public async Task<IActionResult> GetAllTasksCurrentUser()
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(userId))
				return Unauthorized(new { message = "User is not authenticated." });
			
			var result = await _mediator.Send(new GetAllTaskForUserQueries(userId));

			if (result == null || !result.Any())
				return NotFound(new { message = "No tasks found for this user." });
			  
			return Ok(new
			{
				success = true,
				data = result
			});
		}
		 

		[HttpGet("{id}")]    
		public async Task<IActionResult> GetTaskById([FromRoute] string id) 
		{
			var result = await _mediator.Send(new GetTaskByIdQueries(id));
			if (result == null)
			{
				return BadRequest();
			}
			return Ok(result);
		}


 		// DELETE: /tasks/{id}
		[HttpDelete("{id}")]      
		public async Task<IActionResult> DeleteTask(string id)
		{
			var result = await _mediator.Send(new DeleteTaskCommand(id));

			if (!result)
				return NotFound(new { Message = "Task not found" });

			return NoContent();
		}
		 

		// الحصول على جميع المشاركين في المهمة
		// GET api/tasks/{taskId}/collaborators
		[HttpGet("{taskId}/collaborators")]
		public async Task<IActionResult> GetTaskCollaborators(string taskId)
		{
			var result = await _mediator.Send(new GetTaskCollaboratorsQuery(taskId));

			if (result == null || !result.Any())
				return NotFound(new { Message = "No collaborators found for this task" });

			return Ok(result);
		}



		[HttpPost("tasks/{taskId}/collaborators")]
		public async Task<IActionResult> AddCollaborator(string taskId,[FromBody] AddCollaboratorDto Dto)
		{
			var result = await _mediator.Send(new AddTaskCollaboratorCommand(taskId, Dto.UserId));

			if (!result)
				return BadRequest(new
				{
					success = false,
					message = "The user is already assigned to this task."
				});


			return Ok(new
			{
				success = true,
				message = "Collaborator added successfully and notification sent."
			});
		}


		// الحصول على المهام المشتركة لل المستخدم الحالي
		// GET api/collaborators/me/tasks
		[HttpGet("collaborators/me/tasks")]
		public async Task<IActionResult> GetMyCollaboratorTasks()
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (userId == null)
				return Unauthorized();

			var result = await _mediator.Send(new GetTaskCollaboratorCurrentUserQueries(userId));

			return Ok(result);
		}

 
		// إلغاء مشاركة المهمة مع مستخدم
		// DELETE api/tasks/{taskId}/collaborators/{userId}
		[HttpDelete("{taskId}/collaborators/{userId}")]
		public async Task<IActionResult> RemoveCollaborator(string taskId, string userId)
		{
			var result = await _mediator.Send(new RemoveTaskCollaboratorCommand(taskId, userId));

			if (!result)
				return NotFound(new { Message = "Collaborator not found" });

			return Ok(new { Message = "Collaborator removed successfully" });
		}



		// تحديث حالة قبول/رفض المستخدم للمشاركة في المهمة
		// PUT api/tasks/{taskId}/collaborators/{userId}/status
		[HttpPut("tasks/{taskId}/collaborators/{userId}/status")]
		public async Task<IActionResult> UpdateCollaboratorStatus(string taskId, string userId, UpdateCollaboratorStatusDto dto)
		{
			var result = await _mediator.Send(new UpdateTaskCollaboratorStatusCommand(taskId, userId, dto.IsAccepted));

			if (!result)
				return NotFound(new { Message = "Collaborator not found" });

			return Ok(new { Message = "Status updated successfully" });
		}



		[HttpPatch("tasks/{id}/priority")]
		public async Task<IActionResult> UpdateTaskPriority(string id, [FromBody] UpdateTaskPriorityCommand command)
		{
			if (id != command.TaskId)
				return BadRequest(new { Message = "Task ID mismatch" });

			var result = await _mediator.Send(command);
			if (!result)
				return NotFound($"Task with id {id} not found");

 			return Ok(new { Message = "Task priority updated successfully" });

		}



		//// تغيير حالة المهمة
		// PATCH api/tasks/{id}/status
		[HttpPatch("tasks/{id}/status")]
		public async Task<IActionResult> UpdateTaskStatus(string id, [FromBody] UpdateTaskStatusCommand command)
		{
			if (id != command.TaskId)
				return BadRequest(new { Message = "Task ID mismatch" });

			var result = await _mediator.Send(command);

			if (!result)
				return NotFound(new { Message = "Task not found" });

			return Ok(new { Message = "Task status updated successfully" });
		}


		//
		// PATCH api/tasks/{id}/category
		[HttpPatch("{id}/category")]
		public async Task<IActionResult> UpdateTaskCategory(string id, [FromBody] UpdateTaskCategoryCommand command)
		{
			if (id != command.TaskId)
				return BadRequest(new { Message = "Task ID mismatch" });

			var result = await _mediator.Send(command);

			if (!result)
				return NotFound(new { Message = "Task not found" });

			return Ok(new { Message = "Task category updated successfully" });
		}


		//
		// PATCH api/tasks/{id}/recurrence-type
		[HttpPatch("{id}/recurrence-type")]
		public async Task<IActionResult> UpdateTaskRecurrenceType(string id, [FromBody] UpdateTaskRecurrenceTypeCommand command)
		{
			if (id != command.TaskId)
				return BadRequest(new { Message = "Task ID mismatch" });

			var result = await _mediator.Send(command);

			if (!result)
				return NotFound(new { Message = "Task not found" });

			return Ok(new { Message = "Task recurrence type updated successfully" });
		}



 		//// تعيين المهمة لمستخدم
		// PATCH api/tasks/{id}/assign
		[HttpPatch("{id}/assign")]
		public async Task<IActionResult> AssignTask(string id, [FromBody] string userId)
		{
			var currentId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (userId == null)
				return Unauthorized(); 
			var result = await _mediator.Send(new AssignTaskCommand(id,userId,currentId));
			if (!result)
				return NotFound(new { Message = "Task not found" });

			return Ok(new { Message = "Task assigned successfully" });
		}


		// PATCH api/tasks/{id}/deadline
		[HttpPatch("{id}/deadline")]
		public async Task<IActionResult> PatchDeadline([FromRoute] string id, [FromBody] UpdateTaskDeadlineDTO body)
		{
			if (body is null) 
				return BadRequest("Body is required.");

			var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier); // optional
			var ok = await _mediator.Send(new UpdateTaskDeadlineCommand(id, body.NewDueDate, currentUserId));

			if (!ok) 
				return NotFound(new { Message = "Task not found" });

			return Ok(new
			{
				Message = "Deadline updated successfully",
				TaskId = id,
				NewDueDate = body.NewDueDate
			});
		}


		//// الحصول على المهام القادمة (لهذا الأسبوع)
		// GET api/tasks/upcoming
		[HttpGet("upcoming")]
		public async Task<IActionResult> GetUpcomingTasks()
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (userId == null)
				return Unauthorized();

			var result = await _mediator.Send(new GetUpcomingTasksQuery(userId));

			return Ok(result);
		}

		 
		 
	}
}

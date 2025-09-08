using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Commands.NotificationsCommands;
using TaskFlow.Application.DTOs.NotificationsDTO;
using TaskFlow.Application.Queries.NotificationsQueries;
using TaskFlow.Core.Enitites;

namespace TaskFlow.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class NotificationsController : ControllerBase
	{
		private readonly IMediator _mediator;
		private readonly UserManager<ApplicationUser> _userManager;

		public NotificationsController(IMediator mediator,UserManager<ApplicationUser> userManager)
		{
			_mediator = mediator;
			_userManager = userManager;
		}


		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CreateNotificationDTO dto)
		{ 
			var result = await _mediator.Send(new CreateNotificationCommand(dto));
	 
				return Ok(new 
				{
					Message = "Notification created successfully",
					NotificationId = result.Id ,
					Titel=result.Title,
					Massage=result.Message,
 				}); 
 		}


		[HttpGet("{id}")]
		public async Task<IActionResult> GetUserNotifications([FromRoute] string id)
		{
			var notifications = await _mediator.Send(new GetNotificationsByIdQuery(id));
			return Ok(notifications);
		}


 		// الحصول على إشعارات المستخدم
		// GET api/notifications/me
		[HttpGet("me")]
		public async Task<IActionResult> GetMyNotifications()
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (userId == null)
				return Unauthorized();

			var result = await _mediator.Send(new GetNotificationsForUserQuery(userId));
			return Ok(result);
		}



		// PATCH api/notifications/{id}/read
		// الإشعار كمقروء
		[HttpPatch("{id}/read")]
		public async Task<IActionResult> MarkAsRead(string id)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (userId == null)
				return Unauthorized();

			var result = await _mediator.Send(new MarkNotificationAsReadCommand(userId, id));

			if (!result)
				return NotFound(new { Message = "Notification not found" });

			return Ok(new { Message = "Notification marked as read" });
		}


		// PATCH api/notifications/{id}/Unread
		// الإشعار كغير مقروء
		[HttpPatch("{id}/unread")]
		public async Task<IActionResult> MarkAsUnread(string notificationId)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (userId == null)
				return Unauthorized();

			var result = await _mediator.Send(new MarkNotificationAsUnreadCommand(userId, notificationId));

			if (!result)
				return NotFound(new { Message = "Notification not found" });

			return Ok(new { Message = "Notification marked as unread" });
		}


 		// عدد الإشعارات غير المقروءة
		// GET api/notifications/unread-count
		[HttpGet("unread-count")]
		public async Task<IActionResult> GetUnreadCount()
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (userId == null)
				return Unauthorized();

			var count = await _mediator.Send(new GetUnreadNotificationsCountQuery(userId));

			return Ok(new { UnreadCount = count });
		}




		// حذف الإشعار
		// DELETE api/notifications/{id}
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteNotification(string id)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (userId == null)
				return Unauthorized();

			var result = await _mediator.Send(new DeleteNotificationCommand(userId, id));

			if (!result)
				return NotFound(new { Message = "Notification not found" });

			return Ok(new { Message = "Notification deleted successfully" });
		}

		// حذف جميع الإشعارات
		// DELETE api/notifications/me
		[HttpDelete("me")]
		public async Task<IActionResult> DeleteAllForCurrentUser()
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (userId == null)
				return Unauthorized();

			var result = await _mediator.Send(new DeleteAllNotificationsCommand(userId));

			if (!result)
				return NotFound(new { Message = "No notifications found" });

			return Ok(new { Message = "All notifications deleted successfully" });
		}


	}
}

using MediatR;
using Microsoft.AspNetCore.Identity;
using TaskFlow.Application.DTOs.NotificationsDTO;
using TaskFlow.Application.Helpers;
using TaskFlow.Core.Enitites;
using TaskFlow.Core.Enum;
using TaskFlow.Core.Interfaces;
namespace TaskFlow.Application.Commands.NotificationsCommands
{
	public record CreateNotificationCommand(CreateNotificationDTO Dto):IRequest<Notification>;
	
	
	public class CreateNotificationHandler(UserManager<ApplicationUser> _userManager,IRepository<Notification> _notificationRepository,IEmailSender _emailSender) : IRequestHandler<CreateNotificationCommand, Notification>
	{ 

		public async Task<Notification> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
		{
			var user = await _userManager.FindByIdAsync(request.Dto.UserId);
			var notification = new Notification
			{
				UserId = request.Dto.UserId,
				Title = request.Dto.Title,
				Message = request.Dto.Message,
				CreatedAt = DateTime.UtcNow,
				Type = request.Dto.Type,
				IsRead = false
			};
			if (notification.Type==NotificationTypeEnum.Email)
			{
				var body = EmailTemplateNotificationHelper.GenerateNotificationEmail(
					title: notification.Title,
					message: notification.Message,
					createdAt: notification.CreatedAt
				 );
				var subject = $"TaskFlow Notification - {notification.Title}";
				await _emailSender.SendEmailAsync(user.Email, subject, body);
			}
			await _notificationRepository.AddAsync(notification);

			return notification;
		}
	}

}

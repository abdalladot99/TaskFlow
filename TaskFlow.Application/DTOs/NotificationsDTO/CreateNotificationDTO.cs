using TaskFlow.Core.Enum;

namespace TaskFlow.Application.DTOs.NotificationsDTO
{
	public class CreateNotificationDTO
	{
  		public string UserId { get; set; }
		public string Title { get; set; }
		public string Message { get; set; }
		public NotificationTypeEnum Type { get; set; } // email, in-app


	}
}

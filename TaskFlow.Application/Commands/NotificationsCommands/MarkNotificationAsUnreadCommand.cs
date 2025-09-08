using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TaskFlow.Core.Enitites;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.Application.Commands.NotificationsCommands
{
	public record MarkNotificationAsUnreadCommand(string UserId, string NotificationId): IRequest<bool>;

	public class MarkNotificationAsUnreadHandler
		: IRequestHandler<MarkNotificationAsUnreadCommand, bool>
	{
		private readonly IRepository<Notification> _notificationRepository;

		public MarkNotificationAsUnreadHandler(IRepository<Notification> notificationRepository)
		{
			_notificationRepository = notificationRepository;
		}

		public async Task<bool> Handle(MarkNotificationAsUnreadCommand request, CancellationToken cancellationToken)
		{
			var notification = await _notificationRepository.GetByIdAsync(request.NotificationId);
			if (notification == null || notification.UserId != request.UserId)
				return false;

			notification.IsRead = false;
			await _notificationRepository.UpdateAsync(notification.Id, notification);
			return true;
		}
	}
}

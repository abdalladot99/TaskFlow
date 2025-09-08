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
	public record DeleteNotificationCommand(string UserId, string NotificationId): IRequest<bool>;

	public class DeleteNotificationHandler
	: IRequestHandler<DeleteNotificationCommand, bool>
	{
		private readonly IRepository<Notification> _notificationRepository;

		public DeleteNotificationHandler(IRepository<Notification> notificationRepository)
		{
			_notificationRepository = notificationRepository;
		}

		public async Task<bool> Handle(DeleteNotificationCommand request, CancellationToken cancellationToken)
		{
			var notification = await _notificationRepository.GetByIdAsync(request.NotificationId);
			if (notification == null || notification.UserId != request.UserId)
				return false;

			await _notificationRepository.DeleteAsync(notification.Id);
			return true;
		}
	}

}

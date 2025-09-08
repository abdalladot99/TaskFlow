using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Core.Enitites;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.Application.Commands.NotificationsCommands
{
	public record DeleteAllNotificationsCommand(string UserId) : IRequest<bool>;

	public class DeleteAllNotificationsHandler
	: IRequestHandler<DeleteAllNotificationsCommand, bool>
	{
		private readonly IRepository<Notification> _notificationRepository;

		public DeleteAllNotificationsHandler(IRepository<Notification> notificationRepository)
		{
			_notificationRepository = notificationRepository;
		}

		public async Task<bool> Handle(DeleteAllNotificationsCommand request, CancellationToken cancellationToken)
		{
			var notifications = await _notificationRepository.QueryableAsync()
				.Where(n => n.UserId == request.UserId)
				.ToListAsync(cancellationToken);

			if (!notifications.Any())
				return false;

			foreach (var notification in notifications)
			{
				await _notificationRepository.DeleteAsync(notification.Id);
			}

			return true;
		}
	}


}

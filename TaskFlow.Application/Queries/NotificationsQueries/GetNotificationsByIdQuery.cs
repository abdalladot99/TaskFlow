using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.Commands.NotificationsCommands;
using TaskFlow.Core.Enitites;
using TaskFlow.Core.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TaskFlow.Application.Queries.NotificationsQueries
{
	public record GetNotificationsByIdQuery(string Id) : IRequest<NotificationDTO>;

	public class GetUserNotificationsHandler(IRepository<Notification> _notificationRepository, IMapper _mapper)
		: IRequestHandler<GetNotificationsByIdQuery, NotificationDTO>
	{
		 
		public async Task<NotificationDTO> Handle(GetNotificationsByIdQuery request, CancellationToken cancellationToken)
		{
			var notifications = await _notificationRepository.QueryableAsync()
 				.FirstOrDefaultAsync(i=>i.Id==request.Id,cancellationToken);

			if (notifications == null)
				return new NotificationDTO();

			return _mapper.Map<NotificationDTO>(notifications);
		}
	}

}

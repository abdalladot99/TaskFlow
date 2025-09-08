using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Core.Enitites;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.Application.Queries.NotificationsQueries
{
	public record GetNotificationsForUserQuery(string UserId): IRequest<List<NotificationDTO>>;
	
	public class GetNotificationsForUserHandler(IMapper _mapper,IRepository<Notification> _notificationRepository)
		: IRequestHandler<GetNotificationsForUserQuery, List<NotificationDTO>>
	{
	  
		public async Task<List<NotificationDTO>> Handle(GetNotificationsForUserQuery request,CancellationToken cancellationToken)
		{
			var notifications = await _notificationRepository.QueryableAsync()
				.Where(n => n.UserId == request.UserId)
				.OrderByDescending(n => n.CreatedAt)
				.ToListAsync(cancellationToken);

			return _mapper.Map<List<NotificationDTO>>(notifications);
		}
	}


}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Core.Enitites;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.Application.Queries.NotificationsQueries
{
	public record GetUnreadNotificationsCountQuery(string UserId) : IRequest<int>;

	public class GetUnreadNotificationsCountHandler
	: IRequestHandler<GetUnreadNotificationsCountQuery, int>
	{
		private readonly IRepository<Notification> _notificationRepository;

		public GetUnreadNotificationsCountHandler(IRepository<Notification> notificationRepository)
		{
			_notificationRepository = notificationRepository;
		}

		public async Task<int> Handle(GetUnreadNotificationsCountQuery request, CancellationToken cancellationToken)
		{
			return await _notificationRepository.QueryableAsync()
				.Where(n => n.UserId == request.UserId && !n.IsRead)
				.CountAsync(cancellationToken);
		}
	}

}

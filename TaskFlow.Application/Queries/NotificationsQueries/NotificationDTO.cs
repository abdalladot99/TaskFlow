using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Application.Queries.NotificationsQueries
{
	public class NotificationDTO
	{
		public string Id { get; set; }
		public string Title { get; set; }
		public string Message { get; set; }
		public DateTime CreatedAt { get; set; }
		public bool IsRead { get; set; }
		public DateTime ReadAt { get; set; }

	}
}

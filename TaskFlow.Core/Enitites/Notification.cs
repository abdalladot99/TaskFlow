using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Core.Enum;

namespace TaskFlow.Core.Enitites
{
	public class Notification
	{
		public string Id { get; set; }
		public Notification()
		{
			Id = Guid.NewGuid().ToString();
		}
		public string Title { get; set; }
		public string Message { get; set; }
		public DateTime CreatedAt { get; set; }
		public bool IsRead { get; set; }


		public DateTime? ReadAt { get; set; }
		public NotificationTypeEnum Type { get; set; } // email, in-app
		 

		// ده المستخدم اللي الإشعار موجّه له
		public string UserId { get; set; }
		public ApplicationUser User { get; set; }

		// ولو الإشعار مرتبط بمهمة معينة:
		public string? TaskId { get; set; }
		public AppTask Task { get; set; }
	}
}

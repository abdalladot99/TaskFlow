using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Application.DTOs.AnalyticsDTO
{
	public class UserPerformanceDTO
	{
		public string UserId { get; set; } = string.Empty;
		public string UserName { get; set; } = string.Empty;
		public int CompletedTasks { get; set; }
		public int DelayedTasks { get; set; }
		public int InProgressTasks { get; set; }
	}
}

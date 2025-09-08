using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Application.DTOs.AnalyticsDTO
{
	public class CategoryStatsDTO
	{
		public string CategoryName { get; set; } = string.Empty;
		public int TaskCount { get; set; }
	}
}

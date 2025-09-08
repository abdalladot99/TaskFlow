using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Application.DTOs.TaskDTO
{
	public class BigDataViewTaskDTO
	{
		public string Id { get; set; }
		public string Title { get; set; }    
		public string Description { get; set; }	
		public string UserFullName { get; set; }
		public string CategoryName { get; set; }
		public string PriorityName { get; set; }
		public string StatusName { get; set; }   
		public string RecurrenceTypeName { get; set; }
		public DateTime DueDate { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime LastUpdatedAt { get; set; }
		public DateTime? CompletedAt { get; set; }

	}
}

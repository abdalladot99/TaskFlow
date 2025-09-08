using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Application.DTOs.TaskDTO
{
	public class UpdateTaskDTO
	{
		public string TaskId { get; set; }
		public string? Title { get; set; }      
		public string? Description { get; set; } 
 		public string? CategoryId { get; set; }
		public string? PriorityId { get; set; }  
		public string? StatusId { get; set; }    
		public string? RecurrTypeId { get; set; }
	}
}

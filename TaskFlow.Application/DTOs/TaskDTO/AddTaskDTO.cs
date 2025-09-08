using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TaskFlow.Application.DTOs.TaskDTO
{
	public class AddTaskDTO
	{
		public string Title { get; set; }       // عنوان واضح للمهمة
		public string Description { get; set; } // تفاصيل إضافية
		public DateTime DueDate { get; set; }   // آخر موعد للتسليم
		public string CategoryId { get; set; } 
		public string PriorityId { get; set; }  // أولوية المهمة
		public string StatusId { get; set; }    // الحالة الحالية للمهمة
		public string RecurrTypeId { get; set; }
		 
	}
}

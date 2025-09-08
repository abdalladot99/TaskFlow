using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Core.Enitites;

namespace TaskFlow.Application.DTOs.TaskDTO
{
	public class DataTaskDTO
	{
		public string Id { get; set; }
		public string Title { get; set; }       // عنوان واضح للمهمة
		public string Description { get; set; } // تفاصيل إضافية
		public DateTime DueDate { get; set; }   // آخر موعد للتسليم
		//////////////////////////////
		
 		   
	}
}

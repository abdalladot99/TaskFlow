using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Core.Enitites
{
	// نحدد هل المهمة يومية أو أسبوعية أو شهرية
	public class RecurrenceType
	{
		public string Id { get; set; }
		public RecurrenceType()
		{
			Id = Guid.NewGuid().ToString();
		}
		public string Name { get; set; }        // Daily - Weekly - Monthly - Yearly
		public int IntervalInDays { get; set; } // كم يوم بين كل تكرار

		public  ICollection<AppTask> Tasks { get; set; }
	}

}

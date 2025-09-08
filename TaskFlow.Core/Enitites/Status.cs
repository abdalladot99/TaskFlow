using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Core.Enum;

namespace TaskFlow.Core.Enitites
{
	public class Status
	{
		public string Id { get; set; }
		public Status()
		{
			Id = Guid.NewGuid().ToString();
		}
		public string Name { get; set; } // مثل: Pending - InProgress - Completed

		public   ICollection<AppTask> Tasks { get; set; }
	}

}

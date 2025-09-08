using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Core.Enum;

namespace TaskFlow.Core.Enitites
{
	// ده بيوضح مدى أهمية المهمة
	public class Priority
	{
		public string Id { get; set; }
		public Priority()
		{
			Id = Guid.NewGuid().ToString();
		}
		public string Name { get; set; }    // مثلا: High - Medium - Low
		public int Level { get; set; }      // رقم نستخدمه للترتيب الفرعي مثلا High = 1 وهكذا

		public   ICollection<AppTask> Tasks { get; set; }
	}

}

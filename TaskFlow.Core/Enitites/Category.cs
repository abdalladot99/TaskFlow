using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Core.Enitites
{

	// الكلاس ده بيمثل تصنيف أو نوع المهمة (شغل - دراسة - شخصية..)
	public class Category
	{
		public string Id { get; set; }      // مفتاح أساسي
		public Category()
		{
			Id = Guid.NewGuid().ToString();
		}
		public string Name { get; set; } // اسم التصنيف

		// كل تصنيف ممكن يحتوي على أكتر من مهمة
		public ICollection<AppTask> Tasks { get; set; }
	}
}

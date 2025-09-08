using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Core.Enitites
{
	// ده كلاس ربط Many-to-Many بين Task و ApplicationUser
	// يعني لو عايز تضيف أكثر من مستخدم يشاركك في مهمة
	public class TaskCollaborator
	{
		// مفتاح مركّب (مهمة + مستخدم)
		public string TaskId { get; set; } 
		public AppTask Task { get; set; } 

		public string UserId { get; set; }
		public ApplicationUser User { get; set; }

		// لو حبيت تخلي المستخدم لازم يقبل المهمة المشتركة الأول

		public bool IsAccepted { get; set; }
	}

}

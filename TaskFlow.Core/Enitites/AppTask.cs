namespace TaskFlow.Core.Enitites
{
	// ده أهم كلاس في المشروع كله، بيمثل المهمة اللي المستخدم بيعملها
	public class AppTask
	{
		public string Id { get; set; }
		public AppTask()
		{
			Id = Guid.NewGuid().ToString();
		}
		public string Title { get; set; }       // عنوان واضح للمهمة
		public string Description { get; set; } // تفاصيل إضافية
		public DateTime CreatedAt { get; set; } // تاريخ الإنشاء
		public DateTime LastUpdatedAt { get; set; }  // تاريخ آخر تعديل
		public DateTime DueDate { get; set; }   // آخر موعد للتسليم
		public DateTime? CompletedAt { get; set; } // تاريخ الإنجاز (ممكن يكون null لو مش مكتملة)


		// ================= العلاقات ===================

		// صاحب المهمة (اللي أنشأها):
		public string? UserId { get; set; }
		public ApplicationUser User { get; set; }

		// تصنيف المهمة (Category):
		public string CategoryId { get; set; }
		public  Category Category { get; set; }

		// أولوية المهمة:
		public string PriorityId { get; set; }
		public Priority Priority { get; set; }

		// حالة المهمة:
		public string StatusId { get; set; }
		public Status Status { get; set; }

		// نوع التكرار (ممكن تبقى null لو مش متكررة):
		public string? RecurrenceTypeId { get; set; }
		public RecurrenceType RecurrenceType { get; set; }

		// مهمة ممكن يكون فيها أعضاء مشاركين
		public ICollection<TaskCollaborator> Collaborators { get; set; }

		// الإشعارات الخاصة بالمهمة
		public ICollection<Notification> Notifications { get; set; }
	}

}

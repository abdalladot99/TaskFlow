using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TaskFlow.Core.Enitites
{
	public class ApplicationUser : IdentityUser
	{
		public string FullName { get; set; }
		public string? ProfilePictureUrl { get; set; }  
		public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime? LastUpdate { get; set; }
		public DateTime? LastLogin { get; set; }

		public ICollection<AppTask> Tasks { get; set; }  // مهام هو صاحبها
		public ICollection<TaskCollaborator> Collaborations { get; set; } // مهام مشارك فيها
		public ICollection<Notification> Notifications { get; set; } // إشعارات تخصه
		public ICollection<RefreshToken> RefreshTokens { get; set; }// توكنات التحديث الخاصة به


	}
}

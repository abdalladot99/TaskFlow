using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Core.Enitites;

namespace TaskFlow.Infrastructure.Data
{
	public class AppDbContext : IdentityDbContext<ApplicationUser>
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{ }

		// ========= DbSets ===========
		public DbSet<Category> Categories { get; set; }
		public DbSet<Priority> Priorities { get; set; }
		public DbSet<Status> Statuses { get; set; }
		public DbSet<RecurrenceType> RecurrenceTypes { get; set; }
		public DbSet<AppTask> Tasks { get; set; }
		public DbSet<TaskCollaborator> TaskCollaborators { get; set; }
		public DbSet<Notification> Notifications { get; set; }

		public DbSet<RefreshToken> RefreshTokens { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// إعداد المفتاح المركب لجدول TaskCollaborator
			modelBuilder.Entity<TaskCollaborator>()
				.HasKey(tc => new { tc.TaskId, tc.UserId });

			// علاقة TaskCollaborator ↔ Task 
			modelBuilder.Entity<TaskCollaborator>()
				.HasOne(tc => tc.Task)
				.WithMany(t => t.Collaborators)
				.HasForeignKey(tc => tc.TaskId)
				.OnDelete(DeleteBehavior.NoAction);

			// علاقة TaskCollaborator ↔ ApplicationUser 
			modelBuilder.Entity<TaskCollaborator>()
				.HasOne(tc => tc.User)
				.WithMany(u => u.Collaborations)
				.HasForeignKey(tc => tc.UserId)
				.OnDelete(DeleteBehavior.NoAction);

			// ممكن تضيف سييد داتا (Seed Data) هنا لاحقًا لو عايز
		}
	}
}
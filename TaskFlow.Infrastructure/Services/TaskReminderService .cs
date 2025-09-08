using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TaskFlow.Application.Helpers;
using TaskFlow.Core.Enitites;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.Infrastructure.Services
{
	public class TaskReminderService : BackgroundService
	{
		private readonly IServiceProvider _serviceProvider;
 		public TaskReminderService(IServiceProvider serviceProvider )
		{
			_serviceProvider = serviceProvider;
 		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				using var scope = _serviceProvider.CreateScope();
 				var emailService = scope.ServiceProvider.GetRequiredService<IEmailSender>();

				var _taskRepo = scope.ServiceProvider.GetRequiredService<IRepository<AppTask>>();

				var now = DateTime.UtcNow;
				var tasks = await _taskRepo.QueryableAsync()
					.Where(t => t.Status.Name != "Completed"
							 && t.DueDate > now
							 && t.DueDate <= now.AddHours(24))
					.Include(t => t.User)
					.Include(t => t.Priority)
					.Include(t => t.Category)
					.ToListAsync();

				foreach (var task in tasks)
				{
					var body = EmailTemplateTaskReminderHelper.GenerateTaskReminderEmail(
						
						userName:task.User.FullName,
						taskTitle: task.Title,	
 						dueDate: task.DueDate,
						priority: task.Priority.Name,
						category:task.Category.Name, 
 						taskLink: $"https://localhost:44301/api/AppTask/{task.Id}",
						from: "TaskFlow Reminder Service"
 					);
					var subject = "⏰ Task Reminder - Deadline Approaching";
					await emailService.SendEmailAsync(task.User.Email, subject, body);
					 
				}    
				await Task.Delay(TimeSpan.FromHours(22), stoppingToken);
			}
		}
	}
}

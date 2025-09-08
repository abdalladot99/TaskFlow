using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.Helpers;
using TaskFlow.Core.Enitites;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.Application.Commands.TaskCommands
{
	public record AssignTaskCommand(string TaskId,string UserId,string currentId) : IRequest<bool>;

	public class AssignTaskCommandHandler(IEmailSender _emailSender, UserManager<ApplicationUser> _userManager,IRepository<Notification> _notificationRepository,IRepository<AppTask> _taskRepository)
	: IRequestHandler<AssignTaskCommand, bool>
	{
		 
		public async Task<bool> Handle(AssignTaskCommand request, CancellationToken cancellationToken)
		{
			var task = await _taskRepository.QueryableAsync()
				.Include(i=>i.Category)
				.Include(o=>o.Priority)
				.FirstAsync(i=>i.Id==request.TaskId);
			
			if (task == null)
				return false; 

			task.UserId = request.UserId;
			task.LastUpdatedAt = DateTime.UtcNow;

			await _taskRepository.UpdateAsync(task.Id, task);

			var notification = new Notification
			{
				UserId = request.UserId,
				Title = "New Task Assigned",
				Message = $"You have been assigned to the task: {task.Title}",
				CreatedAt = DateTime.UtcNow,
				IsRead = false
			};

			var fromUser = await _userManager.FindByIdAsync(request.currentId);
			if (fromUser!=null) {
				var body = EmailTemplateTaskAssignedHelper.GenerateTaskAssignedEmail(
					taskTitle: task.Title,
					taskDescription: task.Description,
					dueDate: task.DueDate,
					priority: task.Priority.Name,
					category: task.Category.Name,
					from: fromUser.FullName,
					fromEmail: fromUser.Email,
 					taskLink: $"https://localhost:44301/api/AppTask/{task.Id}"
				);

				var emailUser = await _userManager.FindByIdAsync(request.UserId);
				var subject = notification.Title;
				await _emailSender.SendEmailAsync(emailUser.Email, subject, body);
			}

			await _notificationRepository.AddAsync(notification);

			return true;
		}
	}


}

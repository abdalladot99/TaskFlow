using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.Commands.NotificationsCommands;
using TaskFlow.Application.DTOs.NotificationsDTO;
using TaskFlow.Core.Enitites;
using TaskFlow.Core.Enum;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.Application.Commands.TaskCommands
{
	public record AddTaskCollaboratorCommand(string TaskId, string UserId) : IRequest<bool>;

	public class AddTaskCollaboratorHandler(IRepository<AppTask > _taskRepository,IRepository<TaskCollaborator> _collaboratorRepo, IMediator _mediator)
	: IRequestHandler<AddTaskCollaboratorCommand, bool>
	{
	 
		public async Task<bool> Handle(AddTaskCollaboratorCommand request, CancellationToken cancellationToken)
		{
 			var exists = await _collaboratorRepo.QueryableAsync()
				.AnyAsync(c => c.TaskId == request.TaskId && c.UserId == request.UserId);

			if (exists)
				return false;  

 			var collaborator = new TaskCollaborator
			{
				TaskId = request.TaskId,
				UserId = request.UserId,
				IsAccepted = false
			};

			await _collaboratorRepo.AddAsync(collaborator);
			
			var task = await _taskRepository.GetByIdAsync(request.TaskId); 

			CreateNotificationDTO createNotificationDTO = new CreateNotificationDTO
			{
				UserId = request.UserId,  
				Title = "Added as a collaborator in a task",
				Message = $"You have been added as a collaborator in task: {request.TaskId} , Title {task.Title}",
				Type = NotificationTypeEnum.Email

			};
			 

			var result = await _mediator.Send(new CreateNotificationCommand(createNotificationDTO));
			  
			return true;
		}
	}


}

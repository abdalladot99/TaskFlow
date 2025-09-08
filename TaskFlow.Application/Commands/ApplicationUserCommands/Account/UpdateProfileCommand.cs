using MediatR;
using Microsoft.AspNetCore.Identity;
using TaskFlow.Application.Commands.Features.DeletePicture;
using TaskFlow.Application.DTOs.ApplicationUserDTO;
using TaskFlow.Core.Enitites;

namespace TaskFlow.Application.Commands.ApplicationUserCommands.Account
{
	public record UpdateProfileCommand(UpdateProfileDto Dto):IRequest<List<string>>;
	
	public class UpdateProfileHandler : IRequestHandler<UpdateProfileCommand,List<string>> 
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IMediator _mediator;
		public UpdateProfileHandler(UserManager<ApplicationUser> userManager,IMediator mediator)
		{
			_userManager = userManager;
			_mediator = mediator;
		}
		public async Task<List<string>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
		{ 
			var user = await _userManager.FindByIdAsync(request.Dto.Id);
			if (user == null)
				return  new List<string> { "User not found" };
			
			if (!string.IsNullOrEmpty(request.Dto.UserName))
				user.UserName = request.Dto.UserName;

			if (!string.IsNullOrEmpty(request.Dto.FullName))
				user.FullName = request.Dto.FullName;

			if (!string.IsNullOrEmpty(request.Dto.Email))
				user.Email = request.Dto.Email;

			if (!string.IsNullOrEmpty(request.Dto.PhoneNumber))
				user.PhoneNumber = request.Dto.PhoneNumber;

			if (!string.IsNullOrEmpty(request.Dto.ProfilePictureUrl))
			{
				var oldPicture = user.ProfilePictureUrl;

				user.ProfilePictureUrl = request.Dto.ProfilePictureUrl; 

				if (!string.IsNullOrEmpty(oldPicture))
				{
					await _mediator.Send(new DeleteProfilePictureCommand(oldPicture)); 
				}
			}

			user.LastUpdate = DateTime.UtcNow;

			var result = await _userManager.UpdateAsync(user);
			if (!result.Succeeded)
			{
				var errors = result.Errors.Select(e => e.Description).ToList();
				return errors;
			}  
			return  new List<string>();
		}
	}

}

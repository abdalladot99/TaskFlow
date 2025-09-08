using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using TaskFlow.Application.DTOs.ApplicationUserDTO;
using TaskFlow.Core.Enitites;

namespace TaskFlow.Application.Commands.ApplicationUserCommands.Account
{
	public record ChangePasswordCommand(ChangePasswordDto Dto)
	: IRequest<(bool, List<string>)>;



	public class ChangePasswordCommandHandler
	: IRequestHandler<ChangePasswordCommand,(bool,List<string>)>
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public ChangePasswordCommandHandler(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
		{
			_userManager = userManager;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task<(bool, List<string>)> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
		{
 			 
			var user = await _userManager.FindByIdAsync(request.Dto.Id);
			if (user == null)
			{
				return (false, new List<string> { "User not found" });

			}
			// محاولة تغيير الباسورد
			var result = await _userManager.ChangePasswordAsync(user, request.Dto.CurrentPassword, request.Dto.NewPassword);

			if (!result.Succeeded)
			{
				return (false,result.Errors.Select(e=>e.Description).ToList()); 
			}

			return (true, new List<string>());
		}
	}




}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TaskFlow.Application.DTOs.ApplicationUserDTO;
using TaskFlow.Core.Enitites;

namespace TaskFlow.Application.Commands.ApplicationUserCommands.Account
{
	public record ResetPasswordCommand(ResetPasswordDto Dto) : IRequest<(bool Success, string[] Errors)>;


	public class ResetPasswordCommandHandler
	   : IRequestHandler<ResetPasswordCommand, (bool Success, string[] Errors)>
	{
		private readonly UserManager<ApplicationUser> _userManager;

		public ResetPasswordCommandHandler(UserManager<ApplicationUser> userManager)
		{
			_userManager = userManager;
		}

		public async Task<(bool Success, string[] Errors)> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
		{
			var email = request.Dto.Email?.Trim();
			var token = request.Dto.Token;
			var newPassword = request.Dto.NewPassword;

			var user = await _userManager.FindByEmailAsync(email);
			if (user is null)
				return (false, new[] { "Invalid user" });

			// مهم جدًا: لازم تفك الـ Url-Encode لو الفرونت بعت الـ token كما هو من اللينك
			// لو واثق إنه مبعوت كما هو، ممكن تتخطّى السطر التالي
			token = System.Net.WebUtility.UrlDecode(token);

			var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
			if (!result.Succeeded)
				return (false, result.Errors.Select(e => e.Description).ToArray());

			return (true, System.Array.Empty<string>());
		}


	}
}
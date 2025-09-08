using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using TaskFlow.Application.DTOs.ApplicationUserDTO;
using TaskFlow.Core.Enitites;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.Application.Commands.ApplicationUserCommands.Account
{
	public record ForgotPasswordCommand(ForgetPasswordRequestDto Dto) : IRequest<bool>;

	 
	public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, bool>
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IEmailSender _emailSender;
		private readonly IConfiguration _config;

		public ForgotPasswordCommandHandler( UserManager<ApplicationUser> userManager,IEmailSender emailSender,
			IConfiguration config)
		{
			_userManager = userManager;
			_emailSender = emailSender;
			_config = config;
		}

		public async Task<bool> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
		{
			var email = request.Dto.Email?.Trim();
			if (string.IsNullOrWhiteSpace(email))
				return true; 

			var user = await _userManager.FindByEmailAsync(email);
			if (user is null)
				return true; 

 			var token = await _userManager.GeneratePasswordResetTokenAsync(user);
			var encodedToken = Uri.EscapeDataString(token);

			// رابط صفحة الفرونت (ضعه في appsettings: Auth:ResetPasswordUrl)
			// مثال: https://your-frontend.com/reset-password https://localhost:44301/api/AppTask/{task.Id}
			var resetBaseUrl = _config["Auth:ResetPasswordUrl"];
			var resetLink = $"{resetBaseUrl}?email={Uri.EscapeDataString(user.Email)}&token={encodedToken}";

			var subject = "Reset your password";
			var body = $@"
                <p>Hello {(user.FullName ?? user.UserName)},</p>
                <p>You requested to reset your password.</p>
                <p>Click <a href=""{resetLink}"">here</a> to reset it.</p>
                <p>If you didn’t request this, you can safely ignore this email.</p>";

			await _emailSender.SendEmailAsync(user.Email, subject, body);

			return true;
		}

	}
}
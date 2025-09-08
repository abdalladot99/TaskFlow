using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Commands.ApplicationUserCommands.Account;
using TaskFlow.Application.DTOs.ApplicationUserDTO;
using TaskFlow.Core.Enitites;
namespace TaskFlow.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController] 
	[Authorize]  
	public class AccountController : ControllerBase
	{
 
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly ISender _mediator;
		public AccountController(UserManager<ApplicationUser> userManager,ISender mediator)
		{
			_userManager = userManager;
			_mediator = mediator;
		}

 		[HttpGet("me")]
 		public async Task<IActionResult> GetCurrentUser()
		{
 			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (userId == null)
				return Unauthorized();

			var user = await _mediator.Send(new MeCommand(userId)); 
			if (user == null)
				return NotFound();

 			return Ok(new 
			 {
				 Id = user.Id,
				 UserName = user.UserName,
				 FullName = user.FullName,
				 Email = user.Email,
				 PhoneNumber = user.PhoneNumber,
				 CreatedAt = user.CreatedAt,
				 LastLogin = user.LastLogin,
				 LastUpdate = user.LastUpdate,
				 ProfilePictureUrl = user.ProfilePictureUrl
			 });
		}


 
 		[HttpPut("update-profile")]
		public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto model)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (userId == null)
				return Unauthorized();
			model.Id = userId;

			var errors = await _mediator.Send(new UpdateProfileCommand(model));
			if (errors.Any())
			{
				return BadRequest(new { Errors = errors });
			}
			return Ok(new { message = "Profile updated successfully"});
		}
		 

 
		[HttpPut("change-password")]
		public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (userId == null)
				return Unauthorized();
			dto.Id = userId;
			var result = await _mediator.Send(new ChangePasswordCommand(dto));

			if (!result.Item1)
				return BadRequest(result.Item2);

			return Ok("Password changed successfully");
		}
		  

		[AllowAnonymous]
		[HttpPost("forgot-password")]
		public async Task<IActionResult> ForgotPassword([FromBody] ForgetPasswordRequestDto dto)
		{
			await _mediator.Send(new ForgotPasswordCommand(dto));
			return Ok(new { message = "If the email exists, a reset link has been sent." });
		}
		

		[AllowAnonymous]
		[HttpPost("reset-password")]
		public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
		{
			var (success, errors) = await _mediator.Send(new ResetPasswordCommand(dto));
			if (!success)
				return BadRequest(new { message = "Failed to reset password", errors });

			return Ok(new { message = "Password has been reset successfully" });
		}


 		[HttpDelete("me")]
		public async Task<IActionResult> DeleteCurrentUser()
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (userId == null)
				return Unauthorized();

			var result = await _mediator.Send(new DeleteUserCommand(userId));
			if (!result)
				return NotFound(new { Message = "User not found" });

			return Ok(new { Message = "User deleted successfully" });

		}




	}
}

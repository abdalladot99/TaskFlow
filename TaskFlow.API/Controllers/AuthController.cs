using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Commands.ApplicationUserCommands.Auth;
using TaskFlow.Application.Commands.Features.UploadPicture;
using TaskFlow.Application.DTOs.ApplicationUserDTO;
using TaskFlow.Application.DTOs.PictureDTO;

namespace TaskFlow.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class AuthController : ControllerBase
	{

		private readonly ISender _mediator;

		public AuthController(ISender mediator)
		{
			_mediator = mediator;
		}


		//POST /login
		[AllowAnonymous]
		[HttpPost("Login")]
		public async Task<IActionResult> Login([FromBody] LoginUserDTO login)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var (token, expiration,refreshToken, errors) = await _mediator.Send(new LoginCommand(login));

			if (!string.IsNullOrEmpty(token))
				return Ok(new
				{
					AccessToken = token, AccessTokenExpires = expiration,
					RefreshToken=refreshToken.Token, RefreshTokenExpires=refreshToken.Expires
				});

			return BadRequest(new { Errors = errors });
		}



		//POST /register

		[HttpPost("Register")]
		[AllowAnonymous]
		public async Task<IActionResult> Register([FromBody] RegisterUserDTO register)
		{
			if (ModelState.IsValid)
			{
				var (result, Errors) = await _mediator.Send(new RegisterCommand(register));

				if (result)
				{
					return Ok("Account added successed");
				}

				return BadRequest(Errors);
			}
			return BadRequest(ModelState);

		}

		[AllowAnonymous]
		[HttpPost("upload-profile-picture")]
		[Consumes("multipart/form-data")]
		public async Task<IActionResult> UploadProfilePicture([FromForm] UploadProfilePictureDto dto)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var url = await _mediator.Send(new UploadProfilePictureCommand(dto.File));

			return Ok(new { profilePictureUrl = url });
		}



		//POST /refresh-token
		[HttpPost("refresh-token")]
		public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDTO request)
		{
 			var result = await _mediator.Send(new RefreshTokenCommand(request.Token));

			if (!string.IsNullOrEmpty(result.AccessToken))
			{
 				return Ok(new
				{
					AccessToken = result.AccessToken,
					Expiration = result.Expiration,
					RefreshToken = result.RefreshToken
				});
			}

			return BadRequest(result.Errors);
		}

		//POST /logout 
 		[HttpPost("logout")]
		public async Task<IActionResult> Logout([FromBody] LogoutRequestDTO request)
		{
			var result = await _mediator.Send(new LogoutCommand(request.RefreshToken));
			if (!result)
				return BadRequest(new { message = "Invalid or already revoked token" });

			return Ok(new { message = "Logged out successfully" });
		}


		//POST /Logout All Sessions 
 		[HttpPost("logout-all")]
		public async Task<IActionResult> LogoutAll()
		{
 			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;// أو أي Claim انت بتحطه في الـ JWT

 			if (string.IsNullOrEmpty(userId))
				return Unauthorized();

			var result = await _mediator.Send(new LogoutAllCommand(userId));

			if (!result)
				return BadRequest(new { message = "No active sessions found" });

			return Ok(new { message = "All sessions logged out successfully" });
		}




	}
}

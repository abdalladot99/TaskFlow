using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TaskFlow.Application.DTOs.ApplicationUserDTO;
using TaskFlow.Core.Enitites;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.Application.Commands.ApplicationUserCommands.Auth
{
	public record LoginCommand(LoginUserDTO userDTO) : IRequest<(string Token, DateTime Expiration, RefreshToken?, List<string> Errors)>;
	 

	public class LoginHandler
		: IRequestHandler<LoginCommand, (string Token, DateTime Expiration, RefreshToken?, List<string> Errors)>
	{
		private readonly UserManager<Core.Enitites.ApplicationUser> _userManager;
		private readonly IRefreshTokenRepository _refreshToken;
		private readonly IJwtService jwtService;

		public LoginHandler(UserManager<Core.Enitites.ApplicationUser> userManager 
			, IRefreshTokenRepository refreshToken , IJwtService JwtService)
		{
			_userManager = userManager;
			_refreshToken = refreshToken;
			jwtService = JwtService;
		}

		public async Task<(string Token, DateTime Expiration, RefreshToken?, List<string> Errors)> Handle(LoginCommand command, CancellationToken cancellationToken)
		{
			var user = await _userManager.FindByEmailAsync(command.userDTO.Email);
			if (user != null)
			{
				bool found = await _userManager.CheckPasswordAsync(user, command.userDTO.Password);
				if (found) 
				{
					user.LastLogin = DateTime.UtcNow;
					var (tokenString, validTo) =await jwtService.GenerateAccessTokenAsync(user); 
					var refreshToken = jwtService.GenerateRefreshToken(user);

 					if (refreshToken != null) 
					{ 
						await _refreshToken.AddAsync(refreshToken);
						await _refreshToken.SaveChangesAsync(); 
					}

					return (tokenString , validTo, refreshToken , new List<string>());
				}
				return (string.Empty, DateTime.MinValue,new RefreshToken(), new List<string> { "Email or password is not valid" });
			}
			return (string.Empty, DateTime.MinValue, new RefreshToken(), new List<string> { "Email or password is not valid" });
		}


	}

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.Application.Commands.ApplicationUserCommands.Auth
{
	public record RefreshTokenCommand(string Token)
		      : IRequest<(string AccessToken, string RefreshToken, DateTime Expiration, List<string> Errors)>;


	public class RefreshTokenHandler
	 : IRequestHandler<RefreshTokenCommand, (string AccessToken, string RefreshToken, DateTime Expiration, List<string> Errors)>
	{
		private readonly IJwtService _jwtService; // خدمة لتوليد JWT وRefresh Token
		private readonly IRefreshTokenRepository _refreshRepo;

		public RefreshTokenHandler(IJwtService jwtService, IRefreshTokenRepository refreshRepo)
		{
			_jwtService = jwtService;
			_refreshRepo = refreshRepo;
		}

		public async Task<(string AccessToken, string RefreshToken, DateTime Expiration, List<string> Errors)> Handle(
			RefreshTokenCommand command, CancellationToken cancellationToken)
		{
			var refreshToken = await _refreshRepo.GetByTokenAsync(command.Token);

			if (refreshToken == null || refreshToken.IsRevoked)
				return (string.Empty, string.Empty, DateTime.MinValue, new List<string> { "Invalid refresh token" });

			// توليد Access Token جديد
			var  (AccessToken, Expiration) =await _jwtService.GenerateAccessTokenAsync(refreshToken.User);

			// توليد Refresh Token جديد (اختياري)
			var newRefreshToken = _jwtService.GenerateRefreshToken(refreshToken.User);

			await _refreshRepo.UpdateAsync(newRefreshToken);

			return (AccessToken, newRefreshToken.Token, Expiration , new List<string>());
		}
	}


}

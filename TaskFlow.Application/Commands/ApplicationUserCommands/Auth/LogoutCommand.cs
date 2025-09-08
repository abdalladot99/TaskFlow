using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.Application.Commands.ApplicationUserCommands.Auth
{
	public record LogoutCommand(string RefreshToken) : IRequest<bool>;


	public class LogoutHandler : IRequestHandler<LogoutCommand, bool>
	{
 		private readonly IRefreshTokenRepository _refreshTokenRepository;
		public LogoutHandler(IRefreshTokenRepository refreshTokenRepository)
		{
 			_refreshTokenRepository = refreshTokenRepository;
		}

		public async Task<bool> Handle(LogoutCommand request, CancellationToken cancellationToken)
		{
			var refreshToken = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken);
 
			if (refreshToken == null || refreshToken.IsRevoked || refreshToken.Expires < DateTime.UtcNow)
				return false; // ملوش لازمة أصلاً

			refreshToken.IsRevoked = true;
			await _refreshTokenRepository.UpdateAsync(refreshToken);
			await _refreshTokenRepository.SaveChangesAsync(); 
			return true;
		}
	}

}

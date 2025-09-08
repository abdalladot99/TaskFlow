using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.Application.Commands.ApplicationUserCommands.Auth
{
	public record LogoutAllCommand(string UserId) : IRequest<bool>;


	public class LogoutAllHandler : IRequestHandler<LogoutAllCommand, bool>
	{
		private readonly IRefreshTokenRepository _refreshTokenRepository;

		public LogoutAllHandler(IRefreshTokenRepository refreshTokenRepository)
		{
			_refreshTokenRepository = refreshTokenRepository;
		}

		public async Task<bool> Handle(LogoutAllCommand request, CancellationToken cancellationToken)
		{
			var tokens =  _refreshTokenRepository.GetAllRefreshTokens()
				.Where(r => r.UserId == request.UserId && !r.IsRevoked && r.Expires > DateTime.UtcNow)
				.ToList();

			if (!tokens.Any())
				return false;

			foreach (var token in tokens)
			{
				token.IsRevoked = true;
			}

			await _refreshTokenRepository.UpdateListAsync(tokens);

			await _refreshTokenRepository.CleanupExpiredTokensAsync();

			return true;
		}
	}

}

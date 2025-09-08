using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Core.Enitites;

namespace TaskFlow.Core.Interfaces
{
	public interface IRefreshTokenRepository
	{
		Task<RefreshToken> GetByTokenAsync(string token);
		Task AddAsync(RefreshToken refreshToken);
		Task InvalidateAsync(string token); // تعمل Revoke
		Task UpdateAsync(RefreshToken token);
		Task UpdateListAsync(List<RefreshToken> token);
		IQueryable<RefreshToken> GetAllRefreshTokens();
	    Task CleanupExpiredTokensAsync();

		Task SaveChangesAsync();

	}

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Core.Enitites;
using TaskFlow.Core.Interfaces;
using TaskFlow.Infrastructure.Data;

namespace TaskFlow.Infrastructure.Repositories
{
	public class RefreshTokenRepository : IRefreshTokenRepository
	{
		private readonly AppDbContext _context;

		public RefreshTokenRepository(AppDbContext context)
		{
			_context = context;
		}

		public async Task<RefreshToken> GetByTokenAsync(string token)
		{
			return await _context.RefreshTokens.Include(r=>r.User)
				.FirstOrDefaultAsync(r => r.Token == token && !r.IsRevoked);
		}

		public async Task AddAsync(RefreshToken refreshToken)
		{
			await _context.RefreshTokens.AddAsync(refreshToken);
		}

		public async Task InvalidateAsync(string token)
		{
			var refreshToken = await _context.RefreshTokens
				.FirstOrDefaultAsync(r => r.Token == token);

			if (refreshToken != null)
				refreshToken.IsRevoked = true;
		}

		public async Task UpdateAsync(RefreshToken token)
		{
			_context.RefreshTokens.Update(token);
			await SaveChangesAsync();
		} 

		public async Task UpdateListAsync(List<RefreshToken> token)
		{
			_context.RefreshTokens.UpdateRange(token);
			await SaveChangesAsync();
		}

		public IQueryable<RefreshToken>  GetAllRefreshTokens() => _context.RefreshTokens.AsQueryable();

		public async Task SaveChangesAsync()=> await _context.SaveChangesAsync();


		public async Task CleanupExpiredTokensAsync()
		{
			var expiredTokens = _context.RefreshTokens
				.Where(r => r.Expires < DateTime.UtcNow);

			_context.RefreshTokens.RemoveRange(expiredTokens);
			await _context.SaveChangesAsync();
		}


	}

}

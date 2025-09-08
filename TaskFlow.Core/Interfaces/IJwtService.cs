using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Core.Enitites;

namespace TaskFlow.Core.Interfaces
{
	public interface IJwtService
	{
		Task<(string, DateTime)> GenerateAccessTokenAsync(ApplicationUser user); // ينشئ JWT للمستخدم
		RefreshToken GenerateRefreshToken(ApplicationUser user); // ينشئ Refresh Token
		ClaimsPrincipal? GetPrincipalFromExpiredToken(string token); // لو عايز تستخرج بيانات مستخدم من توكن منتهي
	}

}

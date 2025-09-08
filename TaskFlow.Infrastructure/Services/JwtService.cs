using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TaskFlow.Core.Enitites;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.Infrastructure.Services
{
	public class JwtService : IJwtService
	{
		private readonly IConfiguration _config;
		private readonly UserManager<ApplicationUser> _userManager;

		public JwtService(IConfiguration config,UserManager<ApplicationUser> userManager)
		{
			_config = config;
			_userManager = userManager;
		}

		public async Task<(string , DateTime)> GenerateAccessTokenAsync(ApplicationUser user)
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Email,user.Email),
				new Claim(ClaimTypes.NameIdentifier, user.Id),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			};
			var roles = await _userManager.GetRolesAsync(user);
			foreach (var role in roles)
				claims.Add(new Claim(ClaimTypes.Role, role));
			 
			SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]));
			SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			JwtSecurityToken jwtToken = new JwtSecurityToken(
				issuer: _config["JWT:ValidIssuer"],
				audience: _config["JWT:ValidAudience"],
				claims: claims,
				expires: DateTime.UtcNow.AddHours(2),
				signingCredentials: creds
			);

			var tokenString = new JwtSecurityTokenHandler().WriteToken(jwtToken);
			var validTo = jwtToken.ValidTo;
			return (tokenString,validTo) ; 
		}

		 

		public RefreshToken GenerateRefreshToken(ApplicationUser user)
		{
			return new RefreshToken
			{
				Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
				Expires = DateTime.UtcNow.AddDays(10),
				UserId = user.Id,
				IsRevoked = false 
			};
		} 

		public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
		{
			var tokenValidationParameters = new TokenValidationParameters
			{
				ValidateAudience = false,
				ValidateIssuer = false,
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])),
				ValidateLifetime = false // مهم: عشان نسمح بقراءة توكن منتهي
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out _);
			return principal;
		}
	}

}

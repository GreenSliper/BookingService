using Auth.Application.Services;
using Auth.Infrastructure.Data;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Infrastructure.Services
{
	public class JwtService : IJwtService
	{
		private readonly JwtSettings _jwtSettings;

		public JwtService(JwtSettings jwtSettings)
		{
			_jwtSettings = jwtSettings;
		}

		public string GenerateToken(string userId)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = System.Text.Encoding.UTF8.GetBytes(_jwtSettings.Secret);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[]
				{
					new Claim(ClaimTypes.NameIdentifier, userId)
				}),
				Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresInMinutes),
				Issuer = _jwtSettings.Issuer,
				Audience = _jwtSettings.Audience,
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}
	}
}

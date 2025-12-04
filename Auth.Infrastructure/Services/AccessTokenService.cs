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
	public class AccessTokenService : IAccessTokenService
	{
		private readonly JwtSettings _jwtSettings;
		private readonly SymmetricSecurityKey _signingKey;
		private readonly SigningCredentials _signingCredentials;
		public AccessTokenService(JwtSettings jwtSettings)
		{
			_jwtSettings = jwtSettings;
			_signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
			_signingCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256Signature);
		}

		public string GenerateToken(string userId)
		{
			var tokenHandler = new JwtSecurityTokenHandler();

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[]
				{
					new Claim(ClaimTypes.NameIdentifier, userId)
				}),
				Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresInMinutes),
				Issuer = _jwtSettings.Issuer,
				Audience = _jwtSettings.Audience,
				SigningCredentials = _signingCredentials
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}

		public Guid? GetUserIdFromExpiredToken(string token)
		{
			var tokenValidationParameters = new TokenValidationParameters
			{
				ValidateAudience = false,
				ValidateIssuer = false,
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = _signingKey,
				ValidateLifetime = false // <--
			};

			var handler = new JwtSecurityTokenHandler();
			var principal = handler.ValidateToken(token, tokenValidationParameters, out _);

			var id = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			return id is null ? null : Guid.Parse(id);
		}
	}
}

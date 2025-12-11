using Auth.Application.Services;
using Auth.Infrastructure.Data;
using Duende.IdentityServer.Stores;
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
		private readonly SigningCredentials _signingCredentials;
		public AccessTokenService(JwtSettings jwtSettings, ISigningCredentialStore signingCredentialStore)
		{
			_jwtSettings = jwtSettings;
			_signingCredentials = signingCredentialStore.GetSigningCredentialsAsync().Result;
		}

		public string GenerateToken(string userId, IEnumerable<string> userRoles)
		{
			var tokenHandler = new JwtSecurityTokenHandler();

			var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, userId) };
			foreach (var role in userRoles)
				claims.Add(new Claim(ClaimTypes.Role, role));

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
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
				IssuerSigningKey = _signingCredentials.Key,
				ValidateLifetime = false // <--
			};

			var handler = new JwtSecurityTokenHandler();
			var principal = handler.ValidateToken(token, tokenValidationParameters, out _);

			var id = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			return id is null ? null : Guid.Parse(id);
		}
	}
}

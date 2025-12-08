using Auth.Application.Services;
using Auth.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Infrastructure.Services
{
	public class RefreshTokenGenerator : IRefreshTokenGenerator
	{
		private readonly IHasherService _hasherService;

		public RefreshTokenGenerator(IHasherService hasherService)
		{
			_hasherService = hasherService;
		}

		public (RefreshToken entity, string token) Generate(Guid userId)
		{
			// 1. Генерируем случайный токен
			var tokenBytes = RandomNumberGenerator.GetBytes(64);
			string token = Convert.ToBase64String(tokenBytes);
			string salt = _hasherService.GenerateTokenSalt();
			string hash = _hasherService.HashToken(token, salt);

			var refreshToken = new RefreshToken
			{
				Id = Guid.NewGuid(),
				UserId = userId,
				TokenHash = hash,
				TokenSalt = salt,
				CreatedAt = DateTime.UtcNow,
				ExpiresAt = DateTime.UtcNow.AddDays(7)
			};
			return (refreshToken, token);
		}
	}
}

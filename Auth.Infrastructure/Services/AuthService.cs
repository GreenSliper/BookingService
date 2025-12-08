using Auth.Application.Repos;
using Auth.Application.Services;
using Auth.Domain.Entities;
using Auth.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Infrastructure.Services
{
	public class AuthService : IAuthService
	{
		private readonly IUserRepository _userRepository;
		private readonly IHasherService _hasher;
		private readonly IAccessTokenService _accessTokenService;
		private readonly IRefreshTokenGenerator _refreshTokenGenerator;
		private readonly IRefreshTokenRepository _refreshTokenRepository;

		public AuthService(IUserRepository userRepository, IHasherService hasher, 
			IAccessTokenService accessTokenService, IRefreshTokenGenerator refreshTokenGenerator, 
			IRefreshTokenRepository refreshTokenRepository)
		{
			_userRepository = userRepository;
			_hasher = hasher;
			_accessTokenService = accessTokenService;
			_refreshTokenGenerator = refreshTokenGenerator;
			_refreshTokenRepository = refreshTokenRepository;
		}

		public async Task DeleteUserAsync(string email)
		{
			await _userRepository.DeleteByEmailAsync(email);
		}

		public async Task<(string accessToken, string refreshToken)> LoginAsync(string email, string password)
		{
			var user = await _userRepository.GetByEmailAsync(email)
			   ?? throw new UnauthorizedException("Invalid credentials");

			if (!_hasher.VerifyPassword(user.PasswordHash, password))
				throw new UnauthorizedException("Invalid credentials");

			// 1. Создаём access token
			var accessToken = _accessTokenService.GenerateToken(user.Id.ToString(), await _userRepository.GetUserRolesAsync(user.Id));
			// 2. Генерируем refresh token
			(var refreshTokenEntity, string token) = _refreshTokenGenerator.Generate(user.Id);
			await _refreshTokenRepository.AddAsync(refreshTokenEntity);

			return (accessToken, token);
		}

		public async Task<(string accessToken, string refreshToken)> RefreshAsync(string expiredAccessToken, string refreshToken)
		{
			// 1. Получаем userId из access токена (даже если он просрочен)
			var userId = _accessTokenService.GetUserIdFromExpiredToken(expiredAccessToken);
			if (userId == null)
				throw new UnauthorizedAccessException("Invalid access token");

			// 2. Ищем пользователя
			var user = await _userRepository.GetByIdAsync(userId.Value);
			if (user == null)
				throw new UnauthorizedAccessException("User not found");

			// 3. Получаем refresh токены ТОЛЬКО ЭТОГО пользователя
			var tokens = await _refreshTokenRepository.GetByUserIdAsync(user.Id);
			if (tokens == null || !tokens.Any())
				throw new UnauthorizedAccessException("No refresh tokens");

			// 4. Ищем ровно тот токен, который принадлежит пользователю
			var storedToken = tokens
				.FirstOrDefault(t => t.IsActive && _hasher.ValidateToken(refreshToken, t.TokenSalt, t.TokenHash));

			if (storedToken == null)
				throw new UnauthorizedAccessException("Refresh token invalid or expired");

			// 5. Старый refresh token — помечаем как отозванный
			await _refreshTokenRepository.RevokeAsync(storedToken);

			// 6. Создаём новую пару токенов
			var newAccessToken = _accessTokenService.GenerateToken(user.Id.ToString(), await _userRepository.GetUserRolesAsync(user.Id));
			(var refreshTokenEntity, string newRefreshToken) = _refreshTokenGenerator.Generate(user.Id);
			await _refreshTokenRepository.AddAsync(refreshTokenEntity);

			return (newAccessToken, newRefreshToken);
		}

		public async Task RegisterAsync(string email, string password)
		{
			var existingUser = await _userRepository.GetByEmailAsync(email);
			if (existingUser != null)
				throw new EmailTakenException(email); 
			var hashed = _hasher.HashPassword(password);
			var user = new User { 
				Id = Guid.NewGuid(),
				Email = email,
				PasswordHash = hashed,
				CreatedAt = DateTime.UtcNow
			};
			await _userRepository.AddAsync(user);
			await _userRepository.AssignRoleAsync(user.Id, "User");
		}
	}
}

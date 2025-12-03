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
	internal class AuthService : IAuthService
	{
		private readonly IUserRepository _userRepository;
		private readonly IPasswordHasherService _hasher;
		private readonly IJwtService _jwt;

		public AuthService(IUserRepository userRepository, IPasswordHasherService hasher, IJwtService jwt)
		{
			_userRepository = userRepository;
			_hasher = hasher;
			_jwt = jwt;
		}

		public async Task<string?> LoginAsync(string email, string password)
		{
			var user = await _userRepository.GetByEmailAsync(email);
			if (user == null || !_hasher.VerifyPassword(user.PasswordHash, password))
				return null;

			return _jwt.GenerateToken(user.Id.ToString());
		}

		public async Task RegisterAsync(string email, string password)
		{
			var existingUser = await _userRepository.GetByEmailAsync(email);
			if (existingUser != null)
				throw new EmailTakenException(email); 
			var hashed = _hasher.HashPassword(password);
			var user = new User { Email = email, PasswordHash = hashed };
			await _userRepository.AddAsync(user);
		}
	}
}

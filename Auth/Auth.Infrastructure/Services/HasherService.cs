using Auth.Application.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Infrastructure.Services
{
	public class HasherService : IHasherService
	{
		private readonly PasswordHasher<object> _hasher = new PasswordHasher<object>();

		public string HashPassword(string password)
		{
			return _hasher.HashPassword(null, password);
		}

		public bool VerifyPassword(string hashedPassword, string providedPassword)
		{
			var result = _hasher.VerifyHashedPassword(null, hashedPassword, providedPassword);
			return result == PasswordVerificationResult.Success;
		}

		public string GenerateTokenSalt(int size = 16)
		{
			var rng = RandomNumberGenerator.Create();
			var saltBytes = new byte[size];
			rng.GetBytes(saltBytes);
			return Convert.ToBase64String(saltBytes);
		}

		public string HashToken(string token, string salt)
		{
			using var sha = SHA256.Create();
			var combined = Encoding.UTF8.GetBytes(token + salt);
			var hash = sha.ComputeHash(combined);
			return Convert.ToBase64String(hash);
		}

		public bool ValidateToken(string token, string salt, string sourceHash)
		{
			var computed = HashToken(token, salt);
			return computed == sourceHash;
		}
	}
}

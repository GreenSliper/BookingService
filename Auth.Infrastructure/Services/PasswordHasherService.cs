using Auth.Application.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Infrastructure.Services
{
	public class PasswordHasherService : IPasswordHasherService
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
	}
}

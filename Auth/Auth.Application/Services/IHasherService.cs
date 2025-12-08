using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Services
{
	public interface IHasherService
	{
		string HashPassword(string password);
		bool VerifyPassword(string hashedPassword, string providedPassword);
		string GenerateTokenSalt(int size = 16);
		string HashToken(string token, string salt);
		bool ValidateToken(string token, string salt, string sourceHash);
	}
}

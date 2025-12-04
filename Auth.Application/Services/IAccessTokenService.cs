using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Services
{
	public interface IAccessTokenService
	{
		string GenerateToken(string userId);
		Guid? GetUserIdFromExpiredToken(string token);
	}
}

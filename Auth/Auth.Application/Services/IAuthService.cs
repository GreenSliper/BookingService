using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Services
{
	public interface IAuthService
	{
		Task<(string accessToken, string refreshToken)> LoginAsync(string email, string password);
		Task RegisterAsync(string email, string password);
		Task<(string accessToken, string refreshToken)> RefreshAsync(string expiredAccessToken, string refreshToken);
		Task DeleteUserAsync(string email);
	}
}

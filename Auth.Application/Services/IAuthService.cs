using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Services
{
	public interface IAuthService
	{
		Task<string?> LoginAsync(string email, string password);
		Task RegisterAsync(string email, string password);
	}
}

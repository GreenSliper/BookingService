using Auth.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Services
{
	public interface IRefreshTokenRepository
	{
		Task<RefreshToken> GetByHashAsync(string hash);
		Task<IEnumerable<RefreshToken>> GetByUserIdAsync(Guid userId);
		Task AddAsync(RefreshToken token);
		Task RevokeAsync(RefreshToken token);
	}
}

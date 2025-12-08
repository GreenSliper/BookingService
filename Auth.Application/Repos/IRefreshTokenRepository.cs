using Auth.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Repos
{
	public interface IRefreshTokenRepository
	{
		Task<IEnumerable<RefreshToken>> GetByUserIdAsync(Guid userId);
		Task AddAsync(RefreshToken token);
		Task RevokeAsync(RefreshToken token);
		Task DeleteAsync(IEnumerable<RefreshToken> tokens);
	}
}

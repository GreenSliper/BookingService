using Auth.Application.Repos;
using Auth.Domain.Entities;
using Auth.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Infrastructure.Repository
{
	public class RefreshTokenRepository : IRefreshTokenRepository
	{
		private readonly AuthDbContext _context;

		public RefreshTokenRepository(AuthDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<RefreshToken>> GetByUserIdAsync(Guid userId)
		{
			return await _context.RefreshTokens.Where(x=>x.UserId == userId).ToListAsync();
		}

		public async Task AddAsync(RefreshToken token)
		{
			await _context.RefreshTokens.AddAsync(token);
			await _context.SaveChangesAsync();
		}

		public async Task RevokeAsync(RefreshToken token)
		{
			token.RevokedAt = DateTime.UtcNow;
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(IEnumerable<RefreshToken> tokens)
		{
			if (tokens == null || !tokens.Any())
				return;
			_context.RefreshTokens.RemoveRange(tokens);
			await _context.SaveChangesAsync();
		}
	}
}

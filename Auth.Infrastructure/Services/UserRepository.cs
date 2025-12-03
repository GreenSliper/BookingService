using Auth.Application.Services;
using Auth.Domain.Entities;
using Auth.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Infrastructure.Services
{
	public class UserRepository : IUserRepository
	{
		private readonly AuthDbContext _dbContext;

		public UserRepository(AuthDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task AddAsync(User user)
		{
			await _dbContext.Users.AddAsync(user);
			await _dbContext.SaveChangesAsync();
		}

		public async Task<User?> GetByEmailAsync(string email)
		{
			return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
		}

		public async Task<User?> GetByIdAsync(Guid id)
		{
			return await _dbContext.Users.FindAsync(id);
		}

		public async Task UpdateAsync(User user)
		{
			_dbContext.Users.Update(user);
			await _dbContext.SaveChangesAsync();
		}
	}
}

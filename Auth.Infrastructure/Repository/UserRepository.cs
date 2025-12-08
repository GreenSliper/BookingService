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

		public async Task DeleteByEmailAsync(string email)
		{
			var user = await GetByEmailAsync(email);
			if (user == null)
				return;

			_dbContext.Users.Remove(user);
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

		public async Task<List<string>> GetUserRolesAsync(Guid userId)
		{
			return await _dbContext.UserRoles
				.Where(ur => ur.UserId == userId)
				.Select(ur => ur.Role.Name)
				.ToListAsync();
		}

		public async Task AssignRoleAsync(Guid userId, string roleName)
		{
			var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
			if (role == null)
				throw new Exception($"Role {roleName} not found");

			var userRole = new UserRole { UserId = userId, RoleId = role.Id };
			_dbContext.UserRoles.Add(userRole);
			await _dbContext.SaveChangesAsync();
		}
	}
}

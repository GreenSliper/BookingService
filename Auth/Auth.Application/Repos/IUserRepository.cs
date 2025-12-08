using Auth.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Repos
{
	public interface IUserRepository
	{
		Task<User?> GetByIdAsync(Guid id);
		Task<User?> GetByEmailAsync(string email);
		Task AddAsync(User user);
		Task UpdateAsync(User user);
		Task DeleteByEmailAsync(string email);
		Task<List<string>> GetUserRolesAsync(Guid userId);
		Task AssignRoleAsync(Guid userId, string roleName);
	}
}

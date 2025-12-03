using Auth.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Services
{
	public interface IUserRepository
	{
		Task<User?> GetByIdAsync(Guid id);
		Task<User?> GetByEmailAsync(string email);
		Task AddAsync(User user);
		Task UpdateAsync(User user);
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Domain.Entities
{
	public class User
	{
		public Guid Id { get; set; }
		public string Email { get; set; } = null!;
		public string PasswordHash { get; set; } = null!;
		public DateTime CreatedAt { get; set; }
		
		public ICollection<RefreshToken>? RefreshTokens { get; }
		public ICollection<UserRole>? UserRoles { get; }
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Domain.Entities
{
	public class RefreshToken
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public Guid UserId { get; set; }
		public User User { get; set; }

		public string TokenHash { get; set; }
		public string TokenSalt { get; set; }

		public DateTime CreatedAt { get; set; }
		public DateTime ExpiresAt { get; set; }
		public DateTime? RevokedAt { get; set; }

		public bool IsActive => RevokedAt == null && DateTime.UtcNow < ExpiresAt;
	}
}

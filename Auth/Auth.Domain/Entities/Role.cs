using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Domain.Entities
{
	public class Role
	{
		public Guid Id { get; set; }
		public required string Name { get; set; }
		public ICollection<UserRole>? UserRoles { get; set; }
	}
}

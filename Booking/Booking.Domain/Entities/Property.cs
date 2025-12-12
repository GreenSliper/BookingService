using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Entities
{
	public class Property
	{
		public enum Types { Hotel, House, Apartment }

		public Guid Id { get; set; }
		public Guid OwnerId { get; set; }
		public required string Name { get; set; }
		public string? Description { get; set; }
		public required string Address { get; set; }
		public Types Type { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		public ICollection<Room> Rooms { get; }
	}
}

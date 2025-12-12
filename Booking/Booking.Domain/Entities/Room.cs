using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Entities
{
	public class Room
	{
		public Guid Id { get; set; }
		public Guid PropertyId { get; set; }
		public Property Property { get; set; }
		public required string Name { get; set; }
		public string Description { get; set; }
		public int Capacity { get; set; }
		public decimal PricePerNight { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		public ICollection<Reservation> Reservations { get; }
	}
}

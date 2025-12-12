using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Entities
{
	public class Reservation
	{
		public enum Statuses { Pending, Confirmed, Canceled }

		public Guid Id { get; set; }
		public Guid RoomId { get; set; }
		public Room Room { get; set; }

		public Guid UserId { get; set; }
		public DateOnly DateFrom { get; set; }
		public DateOnly DateTo { get; set; }
		public Statuses Status { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	}
}

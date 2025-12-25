using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Application.Dtos
{
	public class RoomDto
	{
		public Guid Id { get; set; }
		public Guid PropertyId { get; set; }
		public required string Name { get; set; }
		public string Description { get; set; }
		public int Capacity { get; set; }
		public decimal PricePerNight { get; set; }
	}
}

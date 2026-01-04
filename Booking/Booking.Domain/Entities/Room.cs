using Booking.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Booking.Domain.Entities
{
	public class Room
	{
		public Guid Id { get; set; }
		public Guid PropertyId { get; set; }
		public Property Property { get; set; }
		public required string Name { get; set; }
		public string? Description { get; set; }
		public int Capacity { get; set; }
		public decimal PricePerNight { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		public ICollection<Reservation> Reservations { get; }

		public static Room Create(Guid propertyId, string name, string description, int capacity, decimal pricePerNight)
		{
			if (string.IsNullOrWhiteSpace(name))
				throw new DomainException("Name is required");
			return new Room()
			{
				PropertyId = propertyId,
				Name = name,
				Description = description,
				Capacity = capacity,
				PricePerNight = pricePerNight
			};
		}

		public void Update(string name, string description, int capacity, decimal pricePerNight)
		{
			if (string.IsNullOrWhiteSpace(name))
				throw new DomainException("Name is required"); 
			Name = name;
			Description = description;
			Capacity = capacity;
			PricePerNight = pricePerNight;
		}
	}
}

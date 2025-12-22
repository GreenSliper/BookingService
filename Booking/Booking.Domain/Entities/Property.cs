using Booking.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Entities
{
	public enum PropertyType { Hotel, House, Apartment }

	public class Property
	{
		public Guid Id { get; set; }
		public Guid OwnerId { get; set; }
		public required string Name { get; set; }
		public string? Description { get; set; }
		public required string Address { get; set; }
		public PropertyType Type { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		public ICollection<Room> Rooms { get; }

		public static Property Create(Guid ownerId, string name, PropertyType type, string address, string? description = null)
		{
			if (string.IsNullOrWhiteSpace(name))
				throw new DomainException("Title is required");
			if (string.IsNullOrWhiteSpace(address))
				throw new DomainException("Address is required");

			return new Property
			{
				Id = Guid.NewGuid(),
				OwnerId = ownerId,
				Name = name,
				Type = type,
				Address = address,
				Description = description
			};
		}

		public void Update(string name, string address)
		{
			if (string.IsNullOrWhiteSpace(name))
				throw new DomainException(nameof(name));

			if (string.IsNullOrWhiteSpace(address))
				throw new DomainException(nameof(address));

			Name = name;
			Address = address;
		}
	}
}

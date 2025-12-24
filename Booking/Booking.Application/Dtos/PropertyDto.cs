using Booking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Booking.Application.Dtos
{
	public class PropertyDto
	{
		public Guid Id { get; init; }
		public required string Name { get; init; }
		public string? Description { get; init; }
		public required string Address { get; init; }
		[JsonConverter(typeof(JsonStringEnumConverter))] // enum -> string for API
		public PropertyType Type { get; init; }

		public Guid OwnerId { get; init; }

		public DateTime CreatedAt { get; init; }
	}
}

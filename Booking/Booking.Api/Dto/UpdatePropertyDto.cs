using Booking.Domain.Entities;

namespace Booking.Application.Dtos
{
	public sealed class UpdatePropertyDto
	{
		public required string Name { get; init; }
		public required string Address { get; init; }
		public PropertyType Type { get; init; }
	}
}

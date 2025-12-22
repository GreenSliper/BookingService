using Booking.Domain.Entities;

namespace Booking.Api.Dto
{
	public sealed class CreatePropertyDto
	{
		public required string Name { get; init; }
		public required string Address { get; init; }
		public PropertyType Type { get; init; }
	}
}

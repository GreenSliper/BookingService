namespace Booking.Api.Dto.Rooms
{
	public class UpdateRoomDto
	{
		public required string Name { get; set; }
		public string Description { get; set; }
		public int Capacity { get; set; }
		public decimal PricePerNight { get; set; }
	}
}

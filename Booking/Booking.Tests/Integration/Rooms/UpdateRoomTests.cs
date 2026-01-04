using Booking.Api.Dto.Rooms;
using Booking.Tests.Integration.Infrastructure;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Tests.Integration.Rooms
{
	public class UpdateRoomTests : IntegrationTestBase
	{
		public UpdateRoomTests(BookingApiFactory factory) : base(factory)
		{
		}

		UpdateRoomDto CreateUpdateDto()
		{
			return new UpdateRoomDto
			{
				Name = "Updated name",
				Capacity = 10,
				PricePerNight = 1000
			};
		}

		[Fact]
		public async Task UpdateRoom_WhenOwner_ReturnsNoContent()
		{
			// Arrange
			var property = await CreatePropertyAsync();
			var room = await CreateRoomAsync(property.Id);

			var dto = CreateUpdateDto();

			// Act
			var response = await Client.PutAsJsonAsync(
				$"/api/properties/{property.Id}/rooms/{room.Id}",
				dto);

			// Assert
			response.StatusCode.ShouldBe(HttpStatusCode.OK);
		}

		[Fact]
		public async Task UpdateRoom_WhenNotOwner_ReturnsForbidden()
		{
			// Arrange
			var property = await CreatePropertyAsync();
			var room = await CreateRoomAsync(property.Id);

			AsUser(Guid.NewGuid());

			// Act
			var response = await Client.PutAsJsonAsync(
				$"/api/properties/{property.Id}/rooms/{room.Id}",
				new { name = "Hack", area = 99 });

			// Assert
			response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
		}

		[Fact]
		public async Task UpdateRoom_WhenRoomNotExists_ReturnsNotFound()
		{
			// Arrange
			var property = await CreatePropertyAsync();

			// Act
			var response = await Client.PutAsJsonAsync(
				$"/api/properties/{property.Id}/rooms/{Guid.NewGuid()}",
				CreateUpdateDto());

			// Assert
			response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
		}

		[Fact]
		public async Task UpdateRoom_WhenPropertyIdInvalid_ReturnsNotFound()
		{
			// Arrange
			var property = await CreatePropertyAsync();
			var room = await CreateRoomAsync(property.Id);
			var anotherProperty = await CreatePropertyAsync();
			// Act
			var response = await Client.PutAsJsonAsync(
				$"/api/properties/{anotherProperty.Id}/rooms/{room.Id}",
				CreateUpdateDto());

			// Assert
			response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
		}
	}
}

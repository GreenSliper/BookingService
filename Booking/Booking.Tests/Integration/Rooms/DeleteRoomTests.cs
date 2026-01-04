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
	public class DeleteRoomTests : IntegrationTestBase
	{
		public DeleteRoomTests(BookingApiFactory factory) : base(factory)
		{
		}

		[Fact]
		public async Task DeleteRoom_WhenOwner_ReturnsNoContent()
		{
			// Arrange
			var property = await CreatePropertyAsync();
			var room = await CreateRoomAsync(property.Id);

			// Act
			var response = await Client.DeleteAsync(
				$"/api/properties/{property.Id}/rooms/{room.Id}");

			// Assert
			response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
		}

		[Fact]
		public async Task DeleteRoom_WhenNotOwner_ReturnsForbidden()
		{
			// Arrange
			var property = await CreatePropertyAsync();
			var room = await CreateRoomAsync(property.Id);

			AsUser(Guid.NewGuid());

			// Act
			var response = await Client.DeleteAsync(
				$"/api/properties/{property.Id}/rooms/{room.Id}");

			// Assert
			response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
		}

		[Fact]
		public async Task DeleteRoom_WhenRoomNotExists_ReturnsNotFound()
		{
			// Arrange
			var property = await CreatePropertyAsync();

			// Act
			var response = await Client.DeleteAsync(
				$"/api/properties/{property.Id}/rooms/{Guid.NewGuid()}");

			// Assert
			response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
		}

		[Fact]
		public async Task DeleteRoom_WhenPropertyIdInvalid_ReturnsNotFound()
		{
			// Arrange
			var property = await CreatePropertyAsync();
			var room = await CreateRoomAsync(property.Id);
			var anotherProperty = await CreatePropertyAsync();
			// Act
			var response = await Client.DeleteAsync(
				$"/api/properties/{anotherProperty.Id}/rooms/{room.Id}");

			// Assert
			response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
		}
	}
}

using Booking.Tests.Integration.Infrastructure;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Tests.Integration.Rooms
{
	public class GetRoomTests : IntegrationTestBase
	{
		public GetRoomTests(BookingApiFactory factory) : base(factory)
		{
		}

		[Fact]
		public async Task GetRoom_WhenExists_ReturnsOk()
		{
			// Arrange
			var property = await CreatePropertyAsync();
			var room = await CreateRoomAsync(property.Id);

			// Act
			var response = await Client.GetAsync(
				$"/api/properties/{property.Id}/rooms/{room.Id}");

			// Assert
			response.StatusCode.ShouldBe(HttpStatusCode.OK);
		}

		[Fact]
		public async Task GetRoom_WhenNotOwner_ReturnsOk()
		{
			// Arrange
			var property = await CreatePropertyAsync();
			var room = await CreateRoomAsync(property.Id);

			AsUser(Guid.NewGuid());

			// Act
			var response = await Client.GetAsync(
				$"/api/properties/{property.Id}/rooms/{room.Id}");

			// Assert
			response.StatusCode.ShouldBe(HttpStatusCode.OK);
		}

		[Fact]
		public async Task GetRoom_WhenRoomNotExists_ReturnsNotFound()
		{
			// Arrange
			var property = await CreatePropertyAsync();

			// Act
			var response = await Client.GetAsync(
				$"/api/properties/{property.Id}/rooms/{Guid.NewGuid()}");

			// Assert
			response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
		}

		[Fact]
		public async Task GetRoom_WhenRoomBelongsToAnotherProperty_ReturnsNotFound()
		{
			// Arrange
			var property1 = await CreatePropertyAsync();
			var property2 = await CreatePropertyAsync();

			var room = await CreateRoomAsync(property1.Id);

			// Act
			var response = await Client.GetAsync(
				$"/api/properties/{property2.Id}/rooms/{room.Id}");

			// Assert
			response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
		}
	}
}

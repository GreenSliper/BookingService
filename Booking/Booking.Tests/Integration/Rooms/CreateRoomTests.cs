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
	public class CreateRoomTests : IntegrationTestBase
	{
		public CreateRoomTests(BookingApiFactory factory) : base(factory)
		{

		}

		[Fact]
		public async Task CreateRoom_WhenOwner_ReturnsCreated()
		{
			// Arrange
			var property = await CreatePropertyAsync();
			await CreateRoomAsync(property.Id);
		}

		[Fact]
		public async Task CreateRoom_WhenNotOwner_ReturnsForbidden()
		{
			// Arrange
			var property = await CreatePropertyAsync();

			AsUser(Guid.NewGuid());

			// Act
			var response = await Client.PostAsJsonAsync(
				$"/api/properties/{property.Id}/rooms",
				CreateRoomDto());

			// Assert
			response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
		}

		[Fact]
		public async Task CreateRoom_WhenPropertyNotExists_ReturnsNotFound()
		{
			// Act
			var response = await Client.PostAsJsonAsync(
				$"/api/properties/{Guid.NewGuid()}/rooms",
				CreateRoomDto());

			// Assert
			response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
		}
	}
}

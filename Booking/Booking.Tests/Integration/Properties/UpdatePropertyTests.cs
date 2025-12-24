using Booking.Application.Dtos;
using Booking.Domain.Entities;
using Booking.Tests.Integration.Infrastructure;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Tests.Integration.Properties
{
	public class UpdatePropertyTests : IntegrationTestBase
	{
		public UpdatePropertyTests(BookingApiFactory factory) : base(factory)
		{
		}

		[Fact]
		public async Task UpdateProperty_UpdatesExistingProperty()
		{
			// Arrange
			var property = await CreatePropertyAsync();

			var updateRequest = new UpdatePropertyDto
			{
				Name = "Updated name",
				Address = "Updated address",
				Type = PropertyType.Apartment
			};

			// Act
			var response = await Client.PutAsJsonAsync(
				$"/api/properties/{property.Id}",
				updateRequest);

			response.EnsureSuccessStatusCode();

			// Assert
			var updated = await response.Content.ReadFromJsonAsync<PropertyDto>();

			updated!.Name.ShouldBe("Updated name");
			updated.Address.ShouldBe("Updated address");
			updated.Type.ShouldBe(PropertyType.Apartment);
		}

		[Fact]
		public async Task UpdateProperty_WhenNotOwner_ReturnsForbidden()
		{
			// Arrange
			var property = await CreatePropertyAsync();
			AsUser(Guid.NewGuid());

			var updateRequest = new UpdatePropertyDto
			{
				Name = "Hack",
				Address = "Hack",
				Type = PropertyType.Hotel
			};

			// Act
			var response = await Client.PutAsJsonAsync(
				$"/api/properties/{property.Id}",
				updateRequest);

			// Assert
			response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
		}
	}
}

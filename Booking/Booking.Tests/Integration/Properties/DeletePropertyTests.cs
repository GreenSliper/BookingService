using Booking.Tests.Integration.Infrastructure;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Tests.Integration.Properties
{
	public class DeletePropertyTests : IntegrationTestBase
	{
		public DeletePropertyTests(BookingApiFactory factory) : base(factory)
		{
		}

		[Fact]
		public async Task DeleteProperty_RemovesProperty()
		{
			// Arrange
			var property = await CreatePropertyAsync();

			// Act
			var response = await Client.DeleteAsync(
				$"/api/properties/{property.Id}");

			response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

			// Assert — check if is indeed deleted
			var getResponse = await Client.GetAsync(
				$"/api/properties/{property.Id}");

			getResponse.StatusCode.ShouldBe(HttpStatusCode.NotFound);
		}

	}
}

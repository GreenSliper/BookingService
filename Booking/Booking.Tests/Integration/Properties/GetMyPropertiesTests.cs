using Booking.Api.Dto;
using Booking.Application.Dtos;
using Booking.Tests.Integration.Infrastructure;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Tests.Integration.Properties
{
	public class GetMyPropertiesTests : IntegrationTestBase
	{
		public GetMyPropertiesTests(BookingApiFactory factory)
			: base(factory) { }

		[Fact]
		public async Task GetMy_ReturnsOnlyMyProperties()
		{
			// arrange
			await CreatePropertyAsync();
			// act
			var response = await Client.GetAsync("/api/properties/my");
			// assert
			var result = await response.Content
				.ReadFromJsonAsync<List<PropertyDto>>();

			result.Count.ShouldBe(1);
		}
	}
}

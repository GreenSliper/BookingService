using Booking.Api.Dto;
using Booking.Application.Commands;
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
	public sealed class CreatePropertyTests : IntegrationTestBase
	{
		public CreatePropertyTests(BookingApiFactory factory)
			: base(factory) { }

		[Fact]
		public async Task CreateProperty_ReturnsCreated()
		{
			var request = new CreatePropertyDto
			{
				Name = "Hotel California",
				Address = "Sunset Boulevard",
				Type = Domain.Entities.PropertyType.Hotel
			};

			var response = await Client.PostAsJsonAsync(
				"/api/properties/create",
				request);

			response.StatusCode.ShouldBe(HttpStatusCode.Created);
		}
	}

}

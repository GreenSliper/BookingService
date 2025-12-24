using Booking.Api.Dto;
using Booking.Application.Dtos;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Tests.Integration.Infrastructure
{
	public abstract class IntegrationTestBase
		: IClassFixture<BookingApiFactory>
	{
		protected readonly HttpClient Client;

		protected IntegrationTestBase(BookingApiFactory factory)
		{
			Client = factory.CreateClient();
		}

		protected void AsUser(Guid userId)
		{
			const string HeaderName = "X-Test-UserId";

			Client.DefaultRequestHeaders.Remove(HeaderName);
			Client.DefaultRequestHeaders.Add(HeaderName, userId.ToString());
		}

		protected async Task<PropertyDto> CreatePropertyAsync()
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
			var property = await response.Content.ReadFromJsonAsync<PropertyDto>();
			property.ShouldNotBeNull();

			return property!;
		}
	}

}

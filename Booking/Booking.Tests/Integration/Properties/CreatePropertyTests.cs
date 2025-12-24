using Booking.Api.Dto;
using Booking.Application.Commands;
using Booking.Application.Dtos;
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
			await CreatePropertyAsync();
		}
	}

}

using Booking.Api;
using Booking.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Booking.Tests.Integration.Infrastructure
{
	public sealed class BookingApiFactory : WebApplicationFactory<Program>
	{
		protected override void ConfigureWebHost(IWebHostBuilder builder)
		{
			builder.ConfigureServices(services =>
			{
				// Remove real DB
				var descriptor = services.Single(
					d => d.ServiceType == typeof(DbContextOptions<BookingDbContext>));

				services.Remove(descriptor);
				var dbName = $"booking_test_{Guid.NewGuid()}";
				services.AddDbContext<BookingDbContext>(options =>
					options.UseInMemoryDatabase(dbName));

				// Fake auth
				services.AddAuthentication("Test")
					.AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
						"Test", _ => { });
			});
		}
	}

}

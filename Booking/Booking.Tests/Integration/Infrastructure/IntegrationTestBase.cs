using System;
using System.Collections.Generic;
using System.Linq;
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
	}

}

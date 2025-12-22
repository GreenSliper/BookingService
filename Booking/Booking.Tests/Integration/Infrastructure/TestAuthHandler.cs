using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Booking.Tests.Integration.Infrastructure
{
	public sealed class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
	{
		public const string UserId = "11111111-1111-1111-1111-111111111111";

		public TestAuthHandler(
			IOptionsMonitor<AuthenticationSchemeOptions> options,
			ILoggerFactory logger,
			UrlEncoder encoder)
		: base(options, logger, encoder)
		{
		}

		protected override Task<AuthenticateResult> HandleAuthenticateAsync()
		{
			var claims = new[]
			{
				new Claim(ClaimTypes.NameIdentifier, UserId)
			};

			var identity = new ClaimsIdentity(claims, "Test");
			var principal = new ClaimsPrincipal(identity);
			var ticket = new AuthenticationTicket(principal, "Test");

			return Task.FromResult(AuthenticateResult.Success(ticket));
		}
	}
}
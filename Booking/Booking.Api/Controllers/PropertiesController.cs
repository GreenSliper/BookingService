using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Api.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/[controller]")]
	public class PropertiesController : ControllerBase
	{
		[HttpGet("test")]
		public IActionResult TestAuth()
		{
			return Ok("ok");
		}

		[HttpPost("test")]
		public async Task<IActionResult> CreateProperty()
		{
			return Ok();
		}
	}
}

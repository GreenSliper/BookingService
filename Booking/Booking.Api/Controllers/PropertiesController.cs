using Booking.Application.Commands;
using Booking.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Booking.Api.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/[controller]")]
	public class PropertiesController : ControllerBase
	{
		private readonly IMediator _mediator;

		public PropertiesController(IMediator mediator)
		{
			_mediator = mediator;
		}

		private Guid GetUserId()
		{
			var sub = User.FindFirstValue(ClaimTypes.NameIdentifier);
			return Guid.Parse(sub);
		}

		[HttpGet("test")]
		public IActionResult TestAuth()
		{
			return Ok("ok");
		}

		[HttpPost("create")]
		public async Task<IActionResult> CreateProperty([FromBody] CreatePropertyCommand command)
		{
			command.OwnerId = GetUserId();
			var result = await _mediator.Send(command);
			return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
		}
		
		[HttpGet("{id}")]
		[AllowAnonymous] // если хочешь, чтобы объект мог смотреть любой
		public async Task<IActionResult> GetById(Guid id)
		{
			var result = await _mediator.Send(new GetPropertyByIdQuery(id));
			return Ok(result);
		}

		[HttpGet("my")]
		public async Task<IActionResult> GetProperties()
		{
			return Ok();
		}
	}
}

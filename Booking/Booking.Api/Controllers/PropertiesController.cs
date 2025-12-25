using Booking.Api.Dto.Properties;
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
		public async Task<IActionResult> CreateProperty([FromBody] CreatePropertyDto dto)
		{
			var command = new CreatePropertyCommand()
			{
				OwnerId = GetUserId(),
				Name = dto.Name,
				Address = dto.Address,
				Type = dto.Type
			};
			var result = await _mediator.Send(command);
			return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
		}
		
		[HttpGet("{id}")]
		[AllowAnonymous] // allow anyone see
		public async Task<IActionResult> GetById(Guid id)
		{
			var result = await _mediator.Send(new GetPropertyByIdQuery(id));
			return Ok(result);
		}

		[HttpGet("my")]
		public async Task<IActionResult> GetProperties(CancellationToken ct)
		{
			var userId = GetUserId(); // extension method

			var result = await _mediator.Send(
				new GetMyPropertiesQuery(userId),
				ct);

			return Ok(result);
		}

		[HttpPut("{id:guid}")]
		public async Task<IActionResult> Update(Guid id, UpdatePropertyDto request, CancellationToken ct)
		{
			var userId = GetUserId();

			var updated = await _mediator.Send(
				new UpdatePropertyCommand(
					id,
					userId,
					request.Name,
					request.Address,
					request.Type),
				ct);

			return Ok(updated);
		}

		[HttpDelete("{id:guid}")]
		public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
		{
			var userId = GetUserId();

			await _mediator.Send(
				new DeletePropertyCommand(id, userId),
				ct);

			return NoContent();
		}
	}
}

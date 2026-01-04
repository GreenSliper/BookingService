using Booking.Api.Dto.Properties;
using Booking.Application.Commands.Properties;
using Booking.Application.Queries.Properties;
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

			var result = await _mediator.Send(
				new GetMyPropertiesQuery(),
				ct);

			return Ok(result);
		}

		[HttpPut("{id:guid}")]
		public async Task<IActionResult> Update(Guid id, UpdatePropertyDto request, CancellationToken ct)
		{
			var updated = await _mediator.Send(
				new UpdatePropertyCommand(
					id,
					request.Name,
					request.Address,
					request.Type),
				ct);

			return Ok(updated);
		}

		[HttpDelete("{id:guid}")]
		public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
		{
			await _mediator.Send(
				new DeletePropertyCommand(id),
				ct);

			return NoContent();
		}
	}
}

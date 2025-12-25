using Booking.Api.Dto.Rooms;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Api.Controllers
{
	[ApiController]
	[Route("api/properties/{propertyId:guid}/rooms")]
	public class RoomsController : ControllerBase
	{
		private readonly IMediator _mediator;

		public RoomsController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpPost]
		public async Task<IActionResult> Create(
			Guid propertyId,
			CreateRoomDto dto)
		{
			var id = await _mediator.Send(new CreateRoomCommand(
				propertyId,
				dto.Name,
				dto.Area));

			return CreatedAtAction(nameof(GetById), new { propertyId, roomId = id }, null);
		}

		[HttpGet("{roomId:guid}")]
		public async Task<ActionResult<RoomDto>> GetById(
			Guid propertyId,
			Guid roomId)
		{
			var room = await _mediator.Send(new GetRoomQuery(propertyId, roomId));
			return Ok(room);
		}

		[HttpGet]
		public async Task<ActionResult<IReadOnlyList<RoomDto>>> GetAll(
			Guid propertyId)
		{
			var rooms = await _mediator.Send(new GetRoomsByPropertyQuery(propertyId));
			return Ok(rooms);
		}

		[HttpPut("{roomId:guid}")]
		public async Task<IActionResult> Update(
			Guid propertyId,
			Guid roomId,
			UpdateRoomDto dto)
		{
			await _mediator.Send(new UpdateRoomCommand(
				propertyId,
				roomId,
				dto.Name,
				dto.Area));

			return NoContent();
		}

		[HttpDelete("{roomId:guid}")]
		public async Task<IActionResult> Delete(
			Guid propertyId,
			Guid roomId)
		{
			await _mediator.Send(new DeleteRoomCommand(propertyId, roomId));
			return NoContent();
		}
	}
}

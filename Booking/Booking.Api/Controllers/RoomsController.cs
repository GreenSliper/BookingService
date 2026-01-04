using Booking.Api.Dto.Rooms;
using Booking.Application.Commands.Rooms;
using Booking.Application.Dtos;
using Booking.Application.Queries.Rooms;
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
			[FromBody] CreateRoomDto dto)
		{
			var roomDto = await _mediator.Send(new CreateRoomCommand(
				propertyId,
				dto.Name,
				dto.Description,
				dto.Capacity,
				dto.PricePerNight));

			return CreatedAtAction(nameof(GetById), new { propertyId, roomId = roomDto.Id }, roomDto);
		}

		[HttpGet("{roomId:guid}")]
		public async Task<ActionResult<RoomDto>> GetById(
			Guid propertyId,
			Guid roomId)
		{
			var room = await _mediator.Send(new GetRoomByIdQuery(propertyId, roomId));
			return Ok(room);
		}

		[HttpGet]
		public async Task<ActionResult<IReadOnlyList<RoomDto>>> GetAll(
			Guid propertyId)
		{
			var rooms = await _mediator.Send(new GetRoomsByPropertyIdQuery(propertyId));
			return Ok(rooms);
		}

		[HttpPut("{roomId:guid}")]
		public async Task<IActionResult> Update(
			Guid propertyId,
			Guid roomId,
			UpdateRoomDto dto)
		{
			var room = await _mediator.Send(new UpdateRoomCommand(
				propertyId,
				roomId,
				dto.Name,
				dto.Description,
				dto.Capacity,
				dto.PricePerNight));

			return Ok(room);
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

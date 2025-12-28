using Booking.Application.Dtos;
using Booking.Application.Exceptions;
using Booking.Application.Extensions;
using Booking.Application.Repos;
using Booking.Application.Services;
using Booking.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Application.Commands.Rooms
{
	public sealed record CreateRoomCommand(
		Guid PropertyId,
		string Name,
		string Description,
		int Capacity,
		decimal PricePerNight)
		: IRequest<RoomDto>;

	public sealed class CreateRoomHandler : IRequestHandler<CreateRoomCommand, RoomDto>
	{
		private readonly IPropertyRepository _properties;
		private readonly IRoomRepository _rooms;
		private readonly ICurrentUser _currentUser;

		public CreateRoomHandler(
			IPropertyRepository properties,
			IRoomRepository rooms,
			ICurrentUser currentUser)
		{
			_properties = properties;
			_rooms = rooms;
			_currentUser = currentUser;
		}

		public async Task<RoomDto> Handle(CreateRoomCommand cmd, CancellationToken ct)
		{
			var property = await _properties.GetByIdAsync(cmd.PropertyId, ct)
				?? throw new NotFoundException($"Property with id {cmd.PropertyId} not found");
			if (property.OwnerId != _currentUser.Id)
				throw new ForbiddenException("Cannot create room for another user property!");

			var room = Room.Create(
				cmd.PropertyId,
				cmd.Name,
				cmd.Description,
				cmd.Capacity,
				cmd.PricePerNight);

			await _rooms.AddAsync(room, ct);

			return room.ToDto();
		}
	}

}

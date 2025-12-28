using Booking.Application.Dtos;
using Booking.Application.Exceptions;
using Booking.Application.Extensions;
using Booking.Application.Repos;
using Booking.Application.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Application.Commands.Rooms
{
	public sealed record UpdateRoomCommand(
		Guid PropertyId,
		Guid RoomId,
		string Name,
		string Description,
		int Capacity,
		decimal PricePerNight) : IRequest<RoomDto>;
	public class UpdateRoomHandler : IRequestHandler<UpdateRoomCommand, RoomDto>
	{
		private readonly IPropertyRepository _propertyRepository;
		private readonly IRoomRepository _roomRepository;
		private readonly ICurrentUser _currentUser;

		public UpdateRoomHandler(IPropertyRepository propertyRepository, IRoomRepository roomRepository, ICurrentUser currentUser)
		{
			_propertyRepository = propertyRepository;
			_roomRepository = roomRepository;
			_currentUser = currentUser;
		}

		public async Task<RoomDto> Handle(UpdateRoomCommand cmd, CancellationToken ct)
		{
			var property = await _propertyRepository.GetByIdAsync(cmd.PropertyId, ct);
			if (property is null)
				throw new NotFoundException($"Property with id {cmd.PropertyId} not found");
			if (property.OwnerId != _currentUser.Id)
				throw new ForbiddenException($"Cannot delete room of another user property");

			var room = await _roomRepository.GetByIdAsync(cmd.RoomId, ct);
			if (room is null)
				throw new NotFoundException("Property not found");

			room.Update(cmd.Name, cmd.Description, cmd.Capacity, cmd.PricePerNight);
			await _roomRepository.UpdateAsync(room, ct);
			return room.ToDto();
		}
	}
}

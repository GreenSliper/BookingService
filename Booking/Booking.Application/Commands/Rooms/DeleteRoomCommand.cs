using Booking.Application.Exceptions;
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
	public sealed record DeleteRoomCommand(Guid PropertyId, Guid RoomId) : IRequest;

	public sealed class DeleteRoomHandler : IRequestHandler<DeleteRoomCommand>
	{
		private readonly IRoomRepository _roomRepository;
		private readonly IPropertyRepository _propertyRepository;
		private readonly ICurrentUser _currentUser;

		public DeleteRoomHandler(IRoomRepository roomRepository, IPropertyRepository propertyRepository, ICurrentUser currentUser)
		{
			_roomRepository = roomRepository;
			_propertyRepository = propertyRepository;
			_currentUser = currentUser;
		}

		public async Task Handle(DeleteRoomCommand cmd, CancellationToken ct)
		{
			var property = await _propertyRepository.GetByIdAsync(cmd.PropertyId, ct);
			if (property is null)
				throw new NotFoundException($"Property with id {cmd.PropertyId} not found");
			if (property.OwnerId != _currentUser.Id)
				throw new ForbiddenException($"Cannot delete room of another user property");

			var room = await _roomRepository.GetByIdAsync(cmd.RoomId, ct);
			if (room is null)
				throw new NotFoundException("Property not found");

			await _roomRepository.DeleteAsync(room, ct);
		}
	}
}

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

namespace Booking.Application.Queries.Rooms
{
	public sealed record GetRoomByIdQuery(Guid PropertyId, Guid RoomId) : IRequest<RoomDto>;

	public sealed class GetRoomByIdHandler : IRequestHandler<GetRoomByIdQuery, RoomDto>
	{
		private readonly IRoomRepository _roomRepository;

		public GetRoomByIdHandler(IRoomRepository roomRepository)
		{
			_roomRepository = roomRepository;
		}

		public async Task<RoomDto> Handle(GetRoomByIdQuery request, CancellationToken cancellationToken)
		{
			var room = await _roomRepository.GetByIdAsync(request.RoomId, cancellationToken);
			if (room == null || request.PropertyId != room.PropertyId)
				throw new NotFoundException($"Room with id {request.RoomId} not found");

			return room.ToDto();
		}
	}
}

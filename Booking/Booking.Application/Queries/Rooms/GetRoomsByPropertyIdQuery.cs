using Booking.Application.Dtos;
using Booking.Application.Exceptions;
using Booking.Application.Extensions;
using Booking.Application.Repos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Application.Queries.Rooms
{
	public sealed record GetRoomsByPropertyIdQuery(Guid PropertyId) : IRequest<IReadOnlyList<RoomDto>>;

	public class GetRoomsByPropertyIdHandler : IRequestHandler<GetRoomsByPropertyIdQuery, IReadOnlyList<RoomDto>>
	{
		private readonly IRoomRepository _roomRepository;
		private readonly IPropertyRepository _propertyRepository;

		public GetRoomsByPropertyIdHandler(IRoomRepository roomRepository, IPropertyRepository propertyRepository)
		{
			_roomRepository = roomRepository;
			_propertyRepository = propertyRepository;
		}

		public async Task<IReadOnlyList<RoomDto>> Handle(GetRoomsByPropertyIdQuery request, CancellationToken cancellationToken)
		{
			if (!await _propertyRepository.ExistsAsync(request.PropertyId, cancellationToken))
				throw new NotFoundException($"Property with id {request.PropertyId} not found");

			var rooms = await _roomRepository.GetByPropertyId(request.PropertyId, cancellationToken);
			return rooms
				.Select(x=>x.ToDto())
				.ToList();
		}
	}
}

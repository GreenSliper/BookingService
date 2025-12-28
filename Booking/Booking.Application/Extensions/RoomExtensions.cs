using Booking.Application.Dtos;
using Booking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Application.Extensions
{
	public static class RoomExtensions
	{
		public static RoomDto ToDto(this Room room)
		{
			return new RoomDto()
			{
				PropertyId = room.PropertyId,
				Name = room.Name,
				Description = room.Description,
				Capacity = room.Capacity,
				PricePerNight = room.PricePerNight,
			};
		}
	}
}

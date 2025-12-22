using Booking.Application.Dtos;
using Booking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Application.Extensions
{
	public static class PropertyExtensions
	{
		public static PropertyDto ToDto(this Property property)
		{
			return new PropertyDto
			{
				Id = property.Id,
				Name = property.Name,
				Description = property.Description,
				Address = property.Address,
				Type = property.Type.ToString(),
				OwnerId = property.OwnerId,
				CreatedAt = property.CreatedAt
			};
		}
	}
}

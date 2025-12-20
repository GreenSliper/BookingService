using Booking.Application.Dtos;
using Booking.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Application.Commands
{
	public class CreatePropertyCommand : IRequest<PropertyDto>
	{
		public Guid OwnerId { get; set; }

		public string Title { get; set; }
		public string Description { get; set; }
		public PropertyType Type { get; set; }
		public string Address { get; set; }
	}
}

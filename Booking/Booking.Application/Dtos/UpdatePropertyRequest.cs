using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Application.Dtos
{
	public sealed class UpdatePropertyRequest
	{
		public required string Title { get; init; }
		public required string Address { get; init; }
	}
}

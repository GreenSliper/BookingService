using Booking.Application.Repos;
using Booking.Domain.Entities;
using Booking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Booking.Infrastructure.Repos
{
	public class RoomRepository : EfRepository<Room>, IRoomRepository
	{
		public RoomRepository(BookingDbContext context) : base(context)
		{
		}

		public async Task<IReadOnlyList<Room>> GetByPropertyId(Guid propertyId, CancellationToken cancellationToken)
		{
			return await _dbSet
				.Where(x => x.PropertyId == propertyId)
				.OrderByDescending(x => x.CreatedAt)
				.ToListAsync(cancellationToken);
		}
	}
}

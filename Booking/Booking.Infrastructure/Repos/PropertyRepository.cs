using Booking.Application.Repos;
using Booking.Domain.Entities;
using Booking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Infrastructure.Repos
{
	public class PropertyRepository : EfRepository<Property>, IPropertyRepository
	{
		public PropertyRepository(BookingDbContext dbContext) : base(dbContext)
		{
		}

		public async Task<IReadOnlyList<Property>> GetByOwnerIdAsync(
			Guid ownerId,
			CancellationToken cancellationToken)
		{
			return await _dbSet
				.Where(x => x.OwnerId == ownerId)
				.OrderByDescending(x => x.CreatedAt)
				.ToListAsync(cancellationToken);
		}

		public async Task<bool> ExistsAsync(
			Guid propertyId,
			CancellationToken cancellationToken)
		{
			return await _dbSet
				.AnyAsync(
					x => x.Id == propertyId,
					cancellationToken);
		}
	}
}
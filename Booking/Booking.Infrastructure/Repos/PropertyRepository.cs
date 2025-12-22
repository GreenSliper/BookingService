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
	public class PropertyRepository : IPropertyRepository
	{
		private readonly BookingDbContext _dbContext;

		public PropertyRepository(BookingDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task AddAsync(
			Property property,
			CancellationToken cancellationToken)
		{
			await _dbContext.Properties.AddAsync(property, cancellationToken);
			await _dbContext.SaveChangesAsync(cancellationToken);
		}

		public async Task<Property?> GetByIdAsync(
			Guid propertyId,
			CancellationToken cancellationToken)
		{
			return await _dbContext.Properties
				.FirstOrDefaultAsync(
					x => x.Id == propertyId,
					cancellationToken);
		}

		public async Task<IReadOnlyList<Property>> GetByOwnerIdAsync(
			Guid ownerId,
			CancellationToken cancellationToken)
		{
			return await _dbContext.Properties
				.Where(x => x.OwnerId == ownerId)
				.OrderByDescending(x => x.CreatedAt)
				.ToListAsync(cancellationToken);
		}

		public async Task<bool> ExistsAsync(
			Guid propertyId,
			Guid ownerId,
			CancellationToken cancellationToken)
		{
			return await _dbContext.Properties
				.AnyAsync(
					x => x.Id == propertyId && x.OwnerId == ownerId,
					cancellationToken);
		}

		public async Task DeleteAsync(
			Property property,
			CancellationToken cancellationToken)
		{
			_dbContext.Properties.Remove(property);
			await _dbContext.SaveChangesAsync(cancellationToken);
		}

		public async Task UpdateAsync(Property property, CancellationToken cancellationToken)
		{
			_dbContext.Properties.Update(property);
			await _dbContext.SaveChangesAsync(cancellationToken);
		}
	}
}

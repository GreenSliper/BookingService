using Booking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Application.Repos
{
	public interface IPropertyRepository
	{
		Task AddAsync(Property property, CancellationToken cancellationToken);
		Task<Property?> GetByIdAsync(Guid propertyId, CancellationToken cancellationToken);
		Task<IReadOnlyList<Property>> GetByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken);
		Task<bool> ExistsAsync(Guid propertyId, Guid ownerId, CancellationToken cancellationToken);
		Task DeleteAsync(Property property, CancellationToken cancellationToken);
	}
}

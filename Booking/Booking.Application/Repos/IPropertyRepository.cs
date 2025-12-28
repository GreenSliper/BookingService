using Booking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Application.Repos
{
	public interface IPropertyRepository : IRepository<Property>
	{
		Task<IReadOnlyList<Property>> GetByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken);
		Task<bool> ExistsAsync(Guid propertyId, CancellationToken cancellationToken);
	}
}

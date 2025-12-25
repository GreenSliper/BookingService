using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Application.Repos
{
	public interface IRepository<T> where T : class
	{
		Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default);
		Task AddAsync(T entity, CancellationToken ct = default);
		Task UpdateAsync(T entity, CancellationToken ct = default);
		Task DeleteAsync(T entity, CancellationToken ct = default);
	}
}

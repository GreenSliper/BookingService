using Booking.Application.Repos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Infrastructure.Repos
{
	public class EfRepository<T> : IRepository<T> where T : class
	{
		protected readonly DbContext _context;
		protected readonly DbSet<T> _dbSet;

		public EfRepository(DbContext context)
		{
			_context = context;
			_dbSet = context.Set<T>();
		}

		public Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
			_dbSet.FindAsync(new object[] { id }, ct).AsTask();

		public Task AddAsync(T entity, CancellationToken ct = default)
		{
			_dbSet.Add(entity);
			return _context.SaveChangesAsync(ct);
		}

		public Task UpdateAsync(T entity, CancellationToken ct = default)
		{
			_dbSet.Update(entity);
			return _context.SaveChangesAsync(ct);
		}

		public Task DeleteAsync(T entity, CancellationToken ct = default)
		{
			_dbSet.Remove(entity);
			return _context.SaveChangesAsync(ct);
		}

		public IQueryable<T> Query() => _dbSet.AsQueryable();
	}
}
using Booking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Infrastructure.Data
{
	public class BookingDbContext : DbContext
	{
		public BookingDbContext(DbContextOptions<BookingDbContext> options)
			: base(options) { }

		public DbSet<Property> Properties { get; set; }
		public DbSet<Reservation> Reservations { get; set; }
		public DbSet<Room> Rooms { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.Entity<Property>(e =>
			{
				e.HasKey(x => x.Id);
				//convert enum to string
				e.Property(x => x.Type).HasConversion<string>();
			});
			builder.Entity<Room>(e =>
			{
				e.HasKey(x => x.Id);
				e.HasOne(x => x.Property)
					.WithMany(x => x.Rooms)
					.HasForeignKey(x => x.PropertyId);
			});
			builder.Entity<Reservation>(e =>
			{
				e.HasKey(x => x.Id);
				//convert enum to string
				e.Property(x => x.Status).HasConversion<string>();
				e.HasOne(x => x.Room)
					.WithMany(x => x.Reservations)
					.HasForeignKey(x => x.RoomId);
			});
		}
	}
}

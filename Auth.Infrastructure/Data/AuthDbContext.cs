using Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Infrastructure.Data
{
	public class AuthDbContext : DbContext
	{
		public AuthDbContext(DbContextOptions<AuthDbContext> options)
			: base(options) { }

		public DbSet<User> Users => Set<User>();
		public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
		public DbSet<Role> Roles => Set<Role>();

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>()
				.HasIndex(x => x.Email)
				.IsUnique();
			modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
			modelBuilder.Entity<UserRole>(entity =>
			{
				//composite key
				entity.HasKey(ur => new { ur.UserId, ur.RoleId });
				// User to UserRole
				entity.HasOne(ur => ur.User)
					  .WithMany(u => u.UserRoles)
					  .HasForeignKey(ur => ur.UserId)
					  .OnDelete(DeleteBehavior.Cascade);
				// Role to UserRole
				entity.HasOne(ur => ur.Role)
					  .WithMany(r => r.UserRoles)
					  .HasForeignKey(ur => ur.RoleId)
					  .OnDelete(DeleteBehavior.Cascade);
			});
		}
	}
}

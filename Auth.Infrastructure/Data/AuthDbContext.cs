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
		public DbSet<UserRole> UserRoles => Set<UserRole>();
		public DbSet<Role> Roles => Set<Role>();

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>()
				.HasIndex(x => x.Email)
				.IsUnique();
			modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
			modelBuilder.Entity<Role>()
				.HasData(
					new Role
					{
						Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
						Name = "Admin"
					},
					new Role
					{
						Id = Guid.Parse("11111111-1111-1111-1111-111111111112"),
						Name = "User"
					}
				);
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

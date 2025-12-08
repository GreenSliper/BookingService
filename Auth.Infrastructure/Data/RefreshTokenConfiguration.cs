using Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Infrastructure.Data
{
	public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
	{
		public void Configure(EntityTypeBuilder<RefreshToken> builder)
		{
			builder.HasKey(x => x.Id);

			builder.Property(x => x.TokenHash).IsRequired();
			builder.Property(x => x.TokenSalt).IsRequired();

			builder.Property(x => x.CreatedAt).IsRequired();
			builder.Property(x => x.ExpiresAt).IsRequired();

			builder.HasOne(x => x.User)
				   .WithMany(u => u.RefreshTokens)
				   .HasForeignKey(x => x.UserId)
				   .OnDelete(DeleteBehavior.Cascade);
		}
	}
}

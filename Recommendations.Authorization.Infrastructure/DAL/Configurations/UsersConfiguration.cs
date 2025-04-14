using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recommendations.Authorization.Core.Types;

namespace Recommendations.Authorization.Infrastructure.DAL.Configurations;

internal sealed class UsersConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.IdUser);
        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(u => u.Surname)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(u => u.Password)
            .IsRequired();
    }
}
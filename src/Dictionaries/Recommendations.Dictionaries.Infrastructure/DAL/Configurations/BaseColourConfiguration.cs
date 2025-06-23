using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recommendations.Dictionaries.Core.Types;

namespace Recommendations.Dictionaries.Infrastructure.DAL.Configurations;

public class BaseColourConfiguration : IEntityTypeConfiguration<BaseColour>
{
    public void Configure(EntityTypeBuilder<BaseColour> builder)
    {
        builder.HasKey(bc => bc.Id);
        
        builder.Property(bc => bc.Name)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.HasIndex(bc => bc.Name).IsUnique();
    }
} 
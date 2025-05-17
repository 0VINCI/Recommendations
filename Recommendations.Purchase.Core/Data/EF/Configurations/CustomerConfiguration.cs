using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recommendations.Purchase.Core.Data.Models;
using Recommendations.Purchase.Core.Types;

namespace Recommendations.Purchase.Core.Data.EF.Configurations;

internal sealed class CustomerConfiguration : IEntityTypeConfiguration<CustomerDbModel>
{
    public void Configure(EntityTypeBuilder<CustomerDbModel> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(c => c.IdCustomer);

        builder.Property(c => c.IdCustomer)
            .ValueGeneratedNever();

        builder.Property(c => c.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(c => c.LastName).IsRequired().HasMaxLength(100);
        builder.Property(c => c.Email).IsRequired().HasMaxLength(200);
        builder.Property(c => c.PhoneNumber).IsRequired().HasMaxLength(20);

        builder.OwnsMany(c => c.Addresses, addresses =>
        {
            addresses.ToTable("CustomerAddresses");
            addresses.WithOwner().HasForeignKey("IdCustomer");
            addresses.Property<Guid>("IdAddress").ValueGeneratedNever();
            addresses.HasKey("IdAddress");
            addresses.Property(a => a.Street).IsRequired().HasMaxLength(200);
            addresses.Property(a => a.City).IsRequired().HasMaxLength(100);
            addresses.Property(a => a.PostalCode).IsRequired().HasMaxLength(20);
            addresses.Property(a => a.Country).IsRequired().HasMaxLength(100);
        });

        builder.OwnsMany(c => c.Payments, payments =>
        {
            payments.ToTable("CustomerPayments");
            payments.WithOwner().HasForeignKey("IdCustomer");
            payments.Property<Guid>("IdPayment").ValueGeneratedNever();
            payments.HasKey("IdPayment");
            payments.Property(p => p.Method)
                .HasConversion<string>()
                .IsRequired();
            payments.Property(p => p.PaymentDate).IsRequired();
            payments.Property(p => p.Details).IsRequired().HasMaxLength(200);
        });
    }
}
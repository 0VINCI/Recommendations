using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recommendations.Purchase.Core.Data.Models;
using Recommendations.Purchase.Core.Types;

namespace Recommendations.Purchase.Core.Data.EF.Configurations;

internal sealed class OrderConfiguration : IEntityTypeConfiguration<OrderDbModel>
{
    public void Configure(EntityTypeBuilder<OrderDbModel> builder)
    {
        builder.ToTable("Orders");

        builder.HasKey(o => o.IdOrder);

        builder.Property(o => o.IdOrder)
            .ValueGeneratedNever();

        builder.Property(o => o.CustomerId)
            .IsRequired();

        builder.Property(o => o.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(o => o.CreatedAt)
            .IsRequired();

        builder.Property(o => o.PaidAt);

        builder.OwnsMany(o => o.Items, items =>
        {
            items.ToTable("OrderItems");

            items.WithOwner().HasForeignKey("IdOrder");

            items.Property<Guid>("IdOrderItem").ValueGeneratedNever();
            items.HasKey("IdOrderItem");

            items.Property(i => i.ProductId).IsRequired();
            items.Property(i => i.ProductName).IsRequired().HasMaxLength(200);
            items.Property(i => i.ProductPrice).IsRequired().HasColumnType("decimal(18,2)");
            items.Property(i => i.Quantity).IsRequired();
        });

        builder.Property(o => o.ShippingAddressId).IsRequired();
        builder.OwnsMany(c => c.Payments, payments =>
        {
            payments.ToTable("OrderPayments");
            payments.WithOwner().HasForeignKey("IdOrder");
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
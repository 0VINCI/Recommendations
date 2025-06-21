using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recommendations.Cart.Core.Types;

namespace Recommendations.Cart.Infrastructure.DAL.Configurations
{
     internal sealed class CartConfiguration : IEntityTypeConfiguration<ShoppingCart>
     {
          public void Configure(EntityTypeBuilder<ShoppingCart> builder)
          {
               builder.HasKey(c => c.IdCart);

               builder.Property(c => c.IdCart)
                    .ValueGeneratedNever();

               builder.Property(c => c.UserId)
                    .IsRequired();

               builder.OwnsMany(c => c.Items, items =>
               {
                    items.ToTable("CartItems");
            
                    items.WithOwner().HasForeignKey("CartId");
            
                    items.Property<Guid>("Id").ValueGeneratedOnAdd();
                    items.HasKey("Id");
            
                    items.Property(i => i.ProductId).IsRequired();
                    items.Property(i => i.Name).IsRequired().HasMaxLength(200);
                    items.Property(i => i.UnitPrice).IsRequired().HasColumnType("decimal(18,2)");
                    items.Property(i => i.Quantity).IsRequired();
               });
          }
     }
}
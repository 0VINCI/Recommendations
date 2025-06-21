using AutoMapper;
using Recommendations.Cart.Core.Types;
using Recommendations.Cart.Shared.DTO;

namespace Recommendations.Cart.Application;

public class CartMappingProfile : Profile
{
    public CartMappingProfile()
    {
        CreateMap<CartItem, CartItemDto>()
            .ForCtorParam("ProductId",  opt => opt.MapFrom(x => x.ProductId))
            .ForCtorParam("Name",       opt => opt.MapFrom(x => x.Name))
            .ForCtorParam("Quantity",   opt => opt.MapFrom(x => x.Quantity))
            .ForCtorParam("UnitPrice",  opt => opt.MapFrom(x => x.UnitPrice))
            .ForCtorParam("Subtotal",   opt => opt.MapFrom(x => x.Subtotal));

        CreateMap<ShoppingCart, ShoppingCartDto>()
            .ForCtorParam("IdCart",    opt => opt.MapFrom(x => x.IdCart))
            .ForCtorParam("CreatedAt", opt => opt.MapFrom(x => x.CreatedAt))
            .ForCtorParam("Total",     opt => opt.MapFrom(x => x.Total))
            .ForCtorParam("Items",     opt => opt.MapFrom(x => x.Items));
    }
}
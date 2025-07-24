using AutoMapper;
using Recommendations.Purchase.Core.Data.Models;
using Recommendations.Purchase.Core.Types;
using Recommendations.Purchase.Shared.DTO;

namespace Recommendations.Purchase.Core.Data;

public class PurchaseMappingProfile : Profile
{
    public PurchaseMappingProfile()
    {
        CreateMap<Customer, CustomerDto>()
            .ForCtorParam("IdCustomer", opt => opt.MapFrom(x => x.IdCustomer))
            .ForCtorParam("UserId", opt => opt.MapFrom(x => x.UserId))
            .ForCtorParam("FirstName", opt => opt.MapFrom(x => x.FirstName))
            .ForCtorParam("LastName", opt => opt.MapFrom(x => x.LastName))
            .ForCtorParam("Email", opt => opt.MapFrom(x => x.Email))
            .ForCtorParam("PhoneNumber", opt => opt.MapFrom(x => x.PhoneNumber));

        CreateMap<Address, AddressDto>()
            .ForCtorParam("IdAddress", opt => opt.MapFrom(x => x.IdAddress))
            .ForCtorParam("Street", opt => opt.MapFrom(x => x.Street))
            .ForCtorParam("City", opt => opt.MapFrom(x => x.City))
            .ForCtorParam("PostalCode", opt => opt.MapFrom(x => x.PostalCode))
            .ForCtorParam("Country", opt => opt.MapFrom(x => x.Country));
        //
        // CreateMap<Order, OrderDto>()
        //     .ForCtorParam("IdOrder",    opt => opt.MapFrom(x => x.IdOrder))
        //     .ForCtorParam("CustomerId", opt => opt.MapFrom(x => x.CustomerId))
        //     .ForCtorParam("ShippingAddressId",     opt => opt.MapFrom(x => x.ShippingAddressId))
        //     .ForCtorParam("Status",     opt => opt.MapFrom(x => x.Status))
        //     .ForCtorParam("CreatedAt", opt => opt.MapFrom(x => x.CreatedAt))
        //     .ForCtorParam("PaidAt",     opt => opt.MapFrom(x => x.PaidAt))
        //     .ForCtorParam("Items",     opt => opt.MapFrom(x => x.Items))
        //     .ForCtorParam("Payments",     opt => opt.MapFrom(x => x.Payments));
        //
        // CreateMap<OrderItem, OrderItemDto>()
        //     .ForCtorParam("ProductId", opt => opt.MapFrom(x => x.ProductId))
        //     .ForCtorParam("ProductName", opt => opt.MapFrom(x => x.ProductName))
        //     .ForCtorParam("ProductPrice", opt => opt.MapFrom(x => x.ProductPrice))
        //     .ForCtorParam("Quantity", opt => opt.MapFrom(x => x.Quantity));

        CreateMap<Customer, CustomerDbModel>();
        CreateMap<CustomerDbModel, Customer>();
        CreateMap<Address, AddressDbModel>();
        CreateMap<AddressDbModel, Address>();
        CreateMap<Order, OrderDbModel>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
            .ForMember(dest => dest.Payments, opt => opt.MapFrom(src => src.Payments));
        CreateMap<OrderDbModel, Order>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
            .ForMember(dest => dest.Payments, opt => opt.MapFrom(src => src.Payments));
        CreateMap<OrderItem, OrderItemDbModel>();
        CreateMap<OrderItemDbModel, OrderItem>();
        CreateMap<Payment, PaymentDbModel>();
        CreateMap<PaymentDbModel, Payment>();
    }
}
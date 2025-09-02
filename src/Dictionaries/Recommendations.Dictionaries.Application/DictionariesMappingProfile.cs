using AutoMapper;
using Recommendations.Dictionaries.Core.Types;
using Recommendations.Dictionaries.Shared.DTO;

namespace Recommendations.Dictionaries.Application;

public class DictionariesMappingProfile : Profile
{
    public DictionariesMappingProfile()
    {
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.SubCategoryName, opt => opt.MapFrom(src => src.SubCategory.Name))
            .ForMember(dest => dest.ArticleTypeName, opt => opt.MapFrom(src => src.ArticleType.Name))
            .ForMember(dest => dest.BaseColourName, opt => opt.MapFrom(src => src.BaseColour.Name))
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images))
            .ForMember(dest => dest.Details, opt => opt.MapFrom(src => src.Details));
        CreateMap<ProductDetails, ProductDetailsDto>();
        CreateMap<ProductImage, ProductImageDto>();

        CreateMap<MasterCategory, MasterCategoryDto>();
        // Usunięto mapowanie na CategoriesDto - nie jest już używane

        CreateMap<SubCategory, SubCategoryDto>();
        // Usunięto mapowanie MasterCategoryName - nie ma takiego pola w DTO
        
        CreateMap<ArticleType, ArticleTypeDto>();
        // Usunięto mapowanie SubCategoryName - nie ma takiego pola w DTO
        
        CreateMap<BaseColour, BaseColourDto>();

        CreateMap<Product, ProductFullDto>()
            .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src))
            .ForMember(dest => dest.Details, opt => opt.MapFrom(src => src.Details))
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images));
    }
} 
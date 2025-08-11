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
        CreateMap<MasterCategory, CategoriesDto>()
            .ForMember(dest => dest.SubCategories, opt => opt.MapFrom(src => src.SubCategories));

        CreateMap<SubCategory, SubCategoryDto>()
            .ForMember(dest => dest.MasterCategoryName, opt => opt.MapFrom(src => src.MasterCategory.Name));
        
        CreateMap<ArticleType, ArticleTypeDto>()
            .ForMember(dest => dest.SubCategoryName, opt => opt.MapFrom(src => src.SubCategory.Name));
        
        CreateMap<BaseColour, BaseColourDto>();

        CreateMap<Product, ProductFullDto>()
            .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src))
            .ForMember(dest => dest.Details, opt => opt.MapFrom(src => src.Details))
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images));
    }
} 
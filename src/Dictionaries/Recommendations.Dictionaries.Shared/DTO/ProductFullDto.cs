namespace Recommendations.Dictionaries.Shared.DTO;

public sealed record ProductFullDto(
    ProductDto Product,
    ProductDetailsDto? Details,
    IReadOnlyCollection<ProductImageDto> Images
); 
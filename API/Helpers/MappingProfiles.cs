using API.Dtos;
using AutoMapper;
using Core.Entities;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(dest => dest.ProductBrand, 
                option => option.MapFrom(source => source.ProductBrand.Name))
                .ForMember(dest => dest.ProductType, 
                option => option.MapFrom(source => source.ProductType.Name))
                .ForMember(dest => dest.PictureUrl, 
                option => option.MapFrom<ProductUrlResolver>());

            
        }
    }
}
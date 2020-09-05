using API.Dtos;
using AutoMapper;
using Core.Entities;
using Microsoft.Extensions.Configuration;

namespace API.Helpers
{
    //purpose:
    //current pictureUrl: images/products/sb-ts1.png
    //goal pictureUrl: https://localhost:5001/images/products/sb-ts1.png
    public class ProductUrlResolver :
    IValueResolver<Product, ProductToReturnDto, string> //string: we want our destination
                                                        //property to be a string(url)
    {
        private readonly IConfiguration _config;
        public ProductUrlResolver(IConfiguration config)
        {
            _config = config;
        }

        public string Resolve(Product source, ProductToReturnDto destination, string destMember, ResolutionContext context)
        {
            if(!string.IsNullOrEmpty(source.PictureUrl))
            {
                return _config["ApiUrl"] + source.PictureUrl;
            }
            return null;
        }
    }
}
using AutoMapper;
using BU.OnlineShop.CatalogService.API.Products.Dtos;
using BU.OnlineShop.CatalogService.Products;

namespace BU.OnlineShop.CatalogService.Profiles
{
    public class CatalogServiceProfile : Profile
    {
        public CatalogServiceProfile()
        {
            CreateMap<Product, ProductDto>();
        }
    }
}

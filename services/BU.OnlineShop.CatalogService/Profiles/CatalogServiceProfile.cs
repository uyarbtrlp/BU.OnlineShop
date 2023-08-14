using AutoMapper;
using BU.OnlineShop.CatalogService.Entities;
using BU.OnlineShop.CatalogService.Models.Products;

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

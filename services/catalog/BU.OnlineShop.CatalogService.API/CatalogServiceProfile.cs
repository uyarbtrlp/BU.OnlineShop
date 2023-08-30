using AutoMapper;
using BU.OnlineShop.CatalogService.API.Dtos.Categories;
using BU.OnlineShop.CatalogService.API.Dtos.Products;
using BU.OnlineShop.CatalogService.Categories;
using BU.OnlineShop.CatalogService.Products;

namespace BU.OnlineShop.CatalogService.Profiles
{
    public class CatalogServiceProfile : Profile
    {
        public CatalogServiceProfile()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<Category, CategoryDto>();
        }
    }
}

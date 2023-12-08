using AutoMapper;
using BU.OnlineShop.BasketService.API.Dtos.Baskets;
using BU.OnlineShop.BasketService.Domain.Baskets;

namespace BU.OnlineShop.BasketService.API
{
    public class BasketServiceProfile : Profile
    {
        public BasketServiceProfile()
        {
            CreateMap<Basket, BasketDto>();
            CreateMap<BasketLine, BasketLineDto>();
        }
    }
}

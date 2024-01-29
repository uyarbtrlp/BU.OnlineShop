using AutoMapper;
using Bu.OnlineShop.BasketService.Abstractions;
using BU.OnlineShop.BasketService.API.Dtos.Baskets;
using BU.OnlineShop.BasketService.Baskets;

namespace BU.OnlineShop.BasketService.API
{
    public class BasketServiceProfile : Profile
    {
        public BasketServiceProfile()
        {
            CreateMap<Basket, BasketDto>();
            CreateMap<BasketLine, BasketLineDto>();
            CreateMap<BasketLineDto, BasketItemEto>();
        }
    }
}

using AutoMapper;
using Bu.OnlineShop.BasketService.Abstractions;
using Bu.OnlineShop.OrderingService.Abstractions;
using BU.OnlineShop.OrderingService.API.Dtos.Orders;
using BU.OnlineShop.OrderingService.Orders;

namespace BU.OnlineShop.OrderingService.API
{
    public class OrderServiceProfile : Profile
    {
        public OrderServiceProfile()
        {
            CreateMap<Order, OrderDto>();
            CreateMap<OrderItem, OrderItemDto>();
            CreateMap<BasketItemEto, OrderItem>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.ProductCode, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.TotalPrice))
                .ForMember(dest => dest.ProductCount, opt => opt.MapFrom(src => src.Count));
        }
    }
}

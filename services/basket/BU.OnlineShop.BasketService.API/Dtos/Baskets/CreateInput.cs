using System.ComponentModel.DataAnnotations;

namespace BU.OnlineShop.BasketService.API.Dtos.Baskets
{
    public class CreateInput
    {
        public Guid UserId { get; set; }
    }
}

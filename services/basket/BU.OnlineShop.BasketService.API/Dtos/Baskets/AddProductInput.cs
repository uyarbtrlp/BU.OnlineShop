using System.ComponentModel.DataAnnotations;

namespace BU.OnlineShop.BasketService.API.Dtos.Baskets
{
    public class AddProductInput
    {
        public Guid ProductId { get; set; }

        [Range(1, int.MaxValue)]
        public int Count { get; set; } = 1;

    }
}

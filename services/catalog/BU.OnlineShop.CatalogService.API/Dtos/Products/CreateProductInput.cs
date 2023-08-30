using System.ComponentModel.DataAnnotations;

namespace BU.OnlineShop.CatalogService.API.Dtos.Products
{
    public class CreateProductInput
    {
        [Required]
        public Guid CategoryId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Code { get; set; }

        public float Price { get; set; }

        public int StockCount { get; set; }
    }
}

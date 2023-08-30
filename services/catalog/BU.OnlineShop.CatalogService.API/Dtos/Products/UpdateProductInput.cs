using System.ComponentModel.DataAnnotations;

namespace BU.OnlineShop.CatalogService.API.Dtos.Products
{
    public class UpdateProductInput
    {
        [Required]
        public Guid CategoryId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public float Price { get; set; }

        [Required]
        public int StockCount { get; set; }
    }
}

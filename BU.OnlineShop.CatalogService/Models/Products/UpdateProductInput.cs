using System.ComponentModel.DataAnnotations;

namespace BU.OnlineShop.CatalogService.Models.Products
{
    public class UpdateProductInput
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public float Price { get; set; }

        [Required]
        public int StockCount { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace BU.OnlineShop.CatalogService.API.Dtos.Categories
{
    public class UpdateCategoryInput
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}

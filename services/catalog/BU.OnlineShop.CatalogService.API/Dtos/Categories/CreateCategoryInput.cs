using System.ComponentModel.DataAnnotations;

namespace BU.OnlineShop.CatalogService.API.Dtos.Categories
{
    public class CreateCategoryInput
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;

namespace BU.OnlineShop.FileService.API.Dtos
{
    public class ChangeNameInput
    {
        [Required]
        public string Name { get; set; }
    }
}

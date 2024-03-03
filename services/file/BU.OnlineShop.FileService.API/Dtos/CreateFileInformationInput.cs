using System.ComponentModel.DataAnnotations;

namespace BU.OnlineShop.FileService.API.Dtos
{
    public class CreateFileInformationInput
    {
        
        public string Name { get; set; }

        [Required]
        public IFormFile File { get; set; }
    }
}

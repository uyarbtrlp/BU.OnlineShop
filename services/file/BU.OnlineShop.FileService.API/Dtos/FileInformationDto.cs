namespace BU.OnlineShop.FileService.API.Dtos
{
    [Serializable]
    public class FileInformationDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string MimeType { get; set; }

        public int Size { get; set; }
    }
}

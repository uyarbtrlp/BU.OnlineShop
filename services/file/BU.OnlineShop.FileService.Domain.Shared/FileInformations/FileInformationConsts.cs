namespace BU.OnlineShop.FileService.Domain.Shared.FileInformations
{
    public static class FileInformationConsts
    {
        public const int MaxNameLength = 256;
        public const int MaxMimeTypeLength = 128;
        public const int MaxSizeLength = int.MaxValue; // 2GB
        public const int MaxContentLength = int.MaxValue; // 2GB
    }
}

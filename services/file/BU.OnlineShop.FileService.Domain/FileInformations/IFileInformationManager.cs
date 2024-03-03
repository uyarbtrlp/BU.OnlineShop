using JetBrains.Annotations;

namespace BU.OnlineShop.FileService.Domain.FileInformations
{
    public interface IFileInformationManager
    {
        Task<FileInformation> CreateAsync(
            [NotNull] string name,
            [NotNull] string mimeType,
            [NotNull] int size,
            [NotNull] byte[] content
            );

         Task<FileInformation> ChangeNameAsync(
            [NotNull] FileInformation fileInformation,
            [NotNull] string newName
            );
       
    }
}

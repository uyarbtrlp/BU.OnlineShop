using BU.OnlineShop.FileService.Domain.Shared.FileInformations;
using JetBrains.Annotations;
using System.Xml.Linq;

namespace BU.OnlineShop.FileService.Domain.FileInformations
{
    public class FileInformationManager : IFileInformationManager
    {
        protected IFileInformationRepository FileInformationRepository { get; set; }

        public FileInformationManager(IFileInformationRepository fileInformationRepository)
        {
            FileInformationRepository = fileInformationRepository;
        }

        public async Task<FileInformation> CreateAsync([NotNull] string name, [NotNull] string mimeType, [NotNull] int size, [NotNull] byte[] content)
        {
            var file = await FileInformationRepository.FindAsync(name);

            if (file != null)
            {
                throw new FileAlreadyExists($"The file named as {name} already exists!");
            }

            return new FileInformation(new Guid(), name, mimeType, size, content);


        }

        public async Task<FileInformation> ChangeNameAsync([NotNull] FileInformation fileInformation, [NotNull] string newName)
        {
            var file = await FileInformationRepository.FindAsync(newName);

            if (file != null)
            {
                throw new FileAlreadyExists($"The file named as {newName} already exists!");
            }

            fileInformation.SetName(newName);

            return fileInformation;
        }
    }
}

using JetBrains.Annotations;

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
                throw new Exception(); //TODO
            }

            return new FileInformation(new Guid(), name, mimeType, size, content);


        }

        public async Task<FileInformation> ChangeNameAsync([NotNull] FileInformation fileInformation, [NotNull] string newName)
        {
            var file = await FileInformationRepository.FindAsync(newName);

            if (file != null)
            {
                throw new Exception(); //TODO
            }

            fileInformation.SetName(newName);

            return fileInformation;
        }
    }
}

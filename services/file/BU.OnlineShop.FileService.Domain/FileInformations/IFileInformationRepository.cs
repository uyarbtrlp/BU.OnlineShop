using BU.OnlineShop.Shared.Repository;

namespace BU.OnlineShop.FileService.Domain.FileInformations
{
    public interface IFileInformationRepository : IRepository<FileInformation>
    {
        Task<IEnumerable<FileInformation>> GetListAsync(
            string name = null);

        Task<long> GetCountAsync(string name = null);

        Task<FileInformation> FindAsync(string name);
    }
}

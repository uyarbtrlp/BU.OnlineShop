using BU.OnlineShop.FileService.Database.EntityFrameworkCore;
using BU.OnlineShop.FileService.Domain.FileInformations;
using BU.OnlineShop.Shared.Extensions;
using BU.OnlineShop.Shared.Repository;
using Microsoft.EntityFrameworkCore;


namespace BU.OnlineShop.FileService.Database.FileInformations
{
    public class FileInformationRepository : Repository<FileInformation>, IFileInformationRepository
    {
        private readonly FileServiceDbContext _dbContext;


        public FileInformationRepository(FileServiceDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<FileInformation>> GetListAsync(
            string name = null
            )
        {
            var query = await GetListQueryAsync(name: name);

            return await query.ToListAsync();
        }

        private async Task<IQueryable<FileInformation>> GetListQueryAsync(
           string name = null)
        {
            var query = _dbContext.Set<FileInformation>()
                .AsNoTracking()
                .WhereIf(!string.IsNullOrEmpty(name), e => e.Name.Contains(name));

            return query;
        }

        public async Task<long> GetCountAsync(string name = null)
        {
            var query = await GetListQueryAsync(name: name);

            return await query.LongCountAsync();
        }

        public Task<FileInformation> FindAsync(string name)
        {
            return _dbContext.Set<FileInformation>().AsNoTracking().FirstOrDefaultAsync(x=>x.Name.Contains(name));
        }
    }
}

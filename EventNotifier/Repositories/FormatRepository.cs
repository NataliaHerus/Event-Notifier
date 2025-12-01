using EventNotifier.Data.Entities.CategoryEntity;
using EventNotifier.Data;
using EventNotifier.Repositories.Interfaces;
using EventNotifier.Data.Entities.FormatEntity;

namespace EventNotifier.Repositories
{
    public class FormatRepository : IFormatRepository
    {
        protected readonly EventNotifierDbContext _dbContext;
        public FormatRepository(EventNotifierDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Format> GetByIdAsync(int id)
        {
            return await _dbContext.Formats.FindAsync(id);
        }

        public async Task<Format> GetFormatByName(string name)
        {
            return _dbContext.Formats.FirstOrDefault(x => x.Name == name);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}

using EventNotifier.Data;
using EventNotifier.Data.Entities;
using EventNotifier.Data.Entities.CategoryEntity;
using EventNotifier.Repositories.Interfaces;

namespace EventNotifier.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        protected readonly EventNotifierDbContext _dbContext;
        public CategoryRepository(EventNotifierDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Category> GetByIdAsync(int id)
        {
            return await _dbContext.Categories.FindAsync(id);
        }

        public async Task<Category> GetCategoryByName(string name)
        {
            return _dbContext.Categories.FirstOrDefault(x => x.Name == name);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}

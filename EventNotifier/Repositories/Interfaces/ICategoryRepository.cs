using EventNotifier.Data.Entities.CategoryEntity;

namespace EventNotifier.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category> GetByIdAsync(int id);
        Task<Category> GetCategoryByName(string name);
        Task<int> SaveChangesAsync();
    }
}

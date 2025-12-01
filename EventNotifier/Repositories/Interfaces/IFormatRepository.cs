using EventNotifier.Data.Entities.CategoryEntity;
using EventNotifier.Data.Entities.FormatEntity;

namespace EventNotifier.Repositories.Interfaces
{
    public interface IFormatRepository
    {
        Task<Format> GetByIdAsync(int id);

        Task<Format> GetFormatByName(string name);
        Task<int> SaveChangesAsync();
    }
}

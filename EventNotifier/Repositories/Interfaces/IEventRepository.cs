using EventNotifier.Data.Entities;

namespace EventNotifier.Repositories.Interfaces
{
    public interface IEventRepository
    {
        Task<Event> AddEventAsync(Event Event);

        Task<Event> UpdateEventAsync(Event Event);

        Task<Event> DeleteAsync(Event Event);

        Task<IEnumerable<Event>> GetAllAsync();

        Task<IEnumerable<Event>> GetClosest();
        Task<Event> GetEventById(int id);

        Task<int> SaveChangesAsync();
    }
}

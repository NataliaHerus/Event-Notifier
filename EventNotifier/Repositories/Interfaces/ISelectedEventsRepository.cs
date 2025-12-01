using EventNotifier.Data.Entities;

namespace EventNotifier.Repositories.Interfaces
{
    public interface ISelectedEventsRepository
    {
        Task<List<SelectedEvents>> GetEventsByUserId(string userId);
        Task<SelectedEvents> AddSelectedEventsAsync(SelectedEvents selectedEvent);
        Task<SelectedEvents> DeleteAsync(SelectedEvents SelectedEvent);
        Task<SelectedEvents> GetSelectedEventById(int id);
        Task<SelectedEvents> GetEvent(string userId, int eventId);
        Task<int> SaveChangesAsync();
    }
}

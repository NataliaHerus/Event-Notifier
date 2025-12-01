using EventNotifier.Data;
using EventNotifier.Data.Entities;
using EventNotifier.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventNotifier.Repositories
{
    public class SelectedEventsRepository : ISelectedEventsRepository
    {
        protected readonly EventNotifierDbContext _dbContext;
        public SelectedEventsRepository(EventNotifierDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<SelectedEvents> AddSelectedEventsAsync(SelectedEvents selectedEvent)
        {
            await _dbContext.SelectedEvents.AddAsync(selectedEvent);
            await _dbContext.SaveChangesAsync();
            return selectedEvent;
        }

        public async Task<SelectedEvents> DeleteAsync(SelectedEvents SelectedEvent)
        {
            await Task.Run(() => _dbContext.SelectedEvents.Remove(SelectedEvent));
            return SelectedEvent;
        }
        public async Task<List<SelectedEvents>> GetEventsByUserId(string userId)
        {
            return  _dbContext.SelectedEvents.Where(x => x.UserId == userId).ToList();
        }

        public async Task<SelectedEvents> GetEvent(string userId, int eventId)
        {
            return await _dbContext.SelectedEvents.FirstOrDefaultAsync(x => x.UserId == userId && x.EventId == eventId);
        }

        public async Task<SelectedEvents> GetSelectedEventById(int id)
        {
            return _dbContext.SelectedEvents.FirstOrDefault(x => x.Id == id);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}

using EventNotifier.Data;
using EventNotifier.Data.Entities;
using EventNotifier.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventNotifier.Repositories
{
    public class EventRepository : IEventRepository
    {
        protected readonly EventNotifierDbContext _dbContext;
        public EventRepository(EventNotifierDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Event> AddEventAsync(Event Event)
        {
            var lastEvent = await _dbContext.Events.OrderBy(e => e.Id).LastOrDefaultAsync();
            Event.Id = lastEvent.Id + 1;
            await _dbContext.Events!.AddAsync(Event);
            await _dbContext.SaveChangesAsync();
            return Event;
        }

        public async Task<Event> DeleteAsync(Event Event)
        {
            await Task.Run(() => _dbContext.Events.Remove(Event));
            return Event;
        }
        public async Task<Event> GetEventById(int id)
        {
            return await _dbContext.Events.Include(e => e.Format).Include(e => e.Category).
                FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Event> UpdateEventAsync(Event Event)
        {
            await Task.Run(() => _dbContext.Entry(Event).State = EntityState.Modified);
            await _dbContext.SaveChangesAsync();
            return Event;
        }

        public async Task<IEnumerable<Event>> GetAllAsync()
        {
            return await _dbContext.Events.Include(e => e.Format).Include(e => e.Category).ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetClosest()
        {
            return await _dbContext.Events.Where(x=> x.StartDate > DateTime.Now && x.Year == DateTime.Now.Year).
                Include(e => e.Format).Include(e => e.Category).OrderBy(x => x.StartDate).ToListAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}

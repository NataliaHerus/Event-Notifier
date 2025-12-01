using EventNotifier.Data.Entities;
using EventNotifier.Data.Entities.CategoryEntity;
using EventNotifier.Data.Entities.EventEntity;
using EventNotifier.Data.Entities.FormatEntity;
using EventNotifier.Data.Entities.SelectedEventsEntity;
using Microsoft.EntityFrameworkCore;

namespace EventNotifier.Data
{
    public class EventNotifierDbContext: DbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<SelectedEvents> SelectedEvents { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Format> Formats { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new EventConfiguration());
            modelBuilder.ApplyConfiguration(new SelectedEventsConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new FormatConfiguration());

        }
        public EventNotifierDbContext(DbContextOptions<EventNotifierDbContext> options)
               : base(options)
        {
        }
    }
}

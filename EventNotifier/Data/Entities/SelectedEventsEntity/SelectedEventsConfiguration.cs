using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventNotifier.Data.Entities.SelectedEventsEntity
{
    public class SelectedEventsConfiguration : IEntityTypeConfiguration<SelectedEvents>
    {
        public void Configure(EntityTypeBuilder<SelectedEvents> builder)
        {

            builder
                   .HasKey(x => x.Id);

            builder
             .Property(x => x.EventId)
             .IsRequired();

            builder
               .HasOne(x => x.Event)
               .WithMany(x => x.SelectedEvents)
               .HasForeignKey(x => x.EventId);
        }
    }
}

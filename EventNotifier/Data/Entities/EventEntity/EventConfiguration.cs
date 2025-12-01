using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventNotifier.Data.Entities.EventEntity
{
    public class EventConfiguration: IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
              .Property(x => x.Name)
              .IsRequired();

            builder
              .Property(x => x.Description)
              .IsRequired();

            builder
              .Property(x => x.Year)
              .IsRequired();

            builder
            .Property(x => x.StartDate)
            .IsRequired();

            builder
            .Property(x => x.EndDate)
            .IsRequired();

            builder
                .HasOne(x => x.Category)
                .WithMany(x => x.Events)
                .HasForeignKey(x => x.CategoryId);

            builder
                .HasOne(x => x.Format)
                .WithMany(x => x.Events)
                .HasForeignKey(x => x.FormatId);

        }
    }
}

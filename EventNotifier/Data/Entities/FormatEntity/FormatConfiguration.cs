using EventNotifier.Data.Entities.CategoryEntity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace EventNotifier.Data.Entities.FormatEntity
{
    public class FormatConfiguration : IEntityTypeConfiguration<Format>
    {
        public void Configure(EntityTypeBuilder<Format> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
             .Property(x => x.Name)
             .IsRequired();
        }

    }
}

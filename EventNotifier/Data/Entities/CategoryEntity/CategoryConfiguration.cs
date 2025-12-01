using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace EventNotifier.Data.Entities.CategoryEntity
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
             .Property(x => x.Name)
             .IsRequired();
        }

    }
}

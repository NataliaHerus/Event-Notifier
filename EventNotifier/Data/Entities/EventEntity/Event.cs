using EventNotifier.Data.Entities.CategoryEntity;
using EventNotifier.Data.Entities.FormatEntity;

namespace EventNotifier.Data.Entities
{
    public class Event
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Year { get; set; }
        public Format? Format { get; set; }
        public int? FormatId { get; set; }
        public Category? Category { get; set; }
        public int? CategoryId { get; set; }
        public ICollection<SelectedEvents>? SelectedEvents { get; set; }
    }
}

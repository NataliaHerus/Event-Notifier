namespace EventNotifier.Data.Entities.FormatEntity
{
    public class Format
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public ICollection<Event>? Events { get; set; }
    }
}

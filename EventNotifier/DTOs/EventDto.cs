namespace EventNotifier.DTOs
{
    public class EventDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Year { get; set; }
        public FormatDto? Format { get; set; }
        public CategoryDto? Category { get; set; }
        public byte[]? Photo { get; set; }
    }
}

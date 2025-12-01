namespace EventNotifier.DTOs
{
    public class EventUpdatedMessage
    {
        public int EventId { get; set; }
        public string NewStartDate { get; set; }
        public string NewEndDate { get; set; }
    }
}

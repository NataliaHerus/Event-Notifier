namespace ScheduleReminders
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public string Description { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Format { get; set; }
        public string Category { get; set; }
        public string ReminderType { get; set; } = string.Empty;

    }
}

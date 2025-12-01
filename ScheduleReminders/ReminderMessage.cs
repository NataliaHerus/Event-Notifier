namespace ScheduleReminders
{
    internal class ReminderMessage
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EventName { get; set; }
        public string Category { get; set; }
        public string Format { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Description { get; set; }
        public string ReminderType { get; set; } = string.Empty;

    }
}
using Microsoft.Azure.WebJobs;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ScheduleReminders
{
    public class RescheduleReminders
    {
        [FunctionName("RescheduleReminders")]
        public async Task Run(
            [QueueTrigger("event-updated", Connection = "AzureWebJobsStorage")] string message,
            [Queue("email-reminders", Connection = "AzureWebJobsStorage")] ICollector<string> reminderQueue,
            ILogger log)
        {
            var evt = JsonSerializer.Deserialize<EventUpdatedMessage>(message);
            if (evt == null)
            {
                log.LogError("Failed to deserialize EventUpdatedMessage");
                return;
            }

            log.LogInformation("Rescheduling reminders for event {id}", evt.EventId);

            var sql = Environment.GetEnvironmentVariable("SqlConnectionStringEventNotifier");

            List<string> userIds = new();

            using (var conn = new SqlConnection(sql))
            using (var cmd = new SqlCommand("SELECT UserId FROM SelectedEvents WHERE EventId = @Id", conn))
            {
                cmd.Parameters.AddWithValue("@Id", evt.EventId);
                conn.Open();

                using SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                    userIds.Add(reader["UserId"].ToString());
            }

            foreach (var userId in userIds)
            {
                var reminderTypes = new[]
                {
                evt.NewStartDate,                                 // День події
                DateTime.Parse(evt.NewStartDate).AddDays(-1).ToString(), // За 1 день
                DateTime.Parse(evt.NewStartDate).AddDays(-7).ToString()  // За 7 днів
            };

                foreach (var remindDate in reminderTypes)
                {
                    var scheduledMessage = new
                    {
                        EventId = evt.EventId,
                        UserId = userId,
                        ScheduledFor = remindDate
                    };

                    reminderQueue.Add(JsonSerializer.Serialize(scheduledMessage));
                }
            }

            log.LogInformation("Reminders rescheduled for event {id}", evt.EventId);
        }
    }

}

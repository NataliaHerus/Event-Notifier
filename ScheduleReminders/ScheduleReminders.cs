using Microsoft.Azure.WebJobs;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ScheduleReminders;

public class ScheduleReminders
{
    public static List<Event> ExecuteEventQuery()
    {
        List<Event> Events = [];

        string eventsQuery = @"
                         SELECT 
                SelectedEvents.UserId, 
                Events.Id AS EventId,
                Events.Name, 
                Events.Description, 
                Events.StartDate, 
                Events.EndDate, 
                Categories.Name AS Category,
                Formats.Name AS Format,

                CASE 
                    WHEN CAST(Events.StartDate AS DATE) = CAST(GETDATE() AS DATE) 
                        THEN 'Today'
                    WHEN CAST(Events.StartDate AS DATE) = CAST(GETDATE() + 1 AS DATE) 
                        THEN '1Day'
                    WHEN CAST(Events.StartDate AS DATE) = CAST(GETDATE() + 7 AS DATE) 
                        THEN '7Days'
                END AS ReminderType

            FROM SelectedEvents
            INNER JOIN Events ON Events.Id = SelectedEvents.EventId
            INNER JOIN Categories ON Categories.Id = Events.CategoryId
            INNER JOIN Formats ON Formats.Id = Events.FormatId
            WHERE 
                CAST(Events.StartDate AS DATE) IN (
                    CAST(GETDATE() AS DATE),
                    CAST(GETDATE() + 1 AS DATE),
                    CAST(GETDATE() + 7 AS DATE)
                )
        ";

        using (SqlConnection sqlConnection = new SqlConnection(
                   Environment.GetEnvironmentVariable("SqlConnectionStringEventNotifier")))
        using (SqlCommand command = new SqlCommand(eventsQuery, sqlConnection))
        {
            sqlConnection.Open();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Event ev = new()
                    {
                        UserId = reader["UserId"].ToString(),
                        Id = Convert.ToInt32(reader["EventId"]),
                        Name = reader["Name"].ToString(),
                        Description = reader["Description"].ToString(),
                        Format = reader["Format"].ToString(),
                        Category = reader["Category"].ToString(),
                        StartDate = reader["StartDate"].ToString(),
                        EndDate = reader["EndDate"].ToString(),
                        ReminderType = reader["ReminderType"].ToString()
                };

                    Events.Add(ev);
                }
            }
        }

        return Events;
    }

    public static List<User> ExecuteUserQuery(List<Event> Events)
    {
        List<User> Users = new List<User>();

        using (SqlConnection sqlConnection = new SqlConnection(
                   Environment.GetEnvironmentVariable("SqlConnectionStringIdentityServer")))
        using (SqlCommand command = sqlConnection.CreateCommand())
        {
            sqlConnection.Open();

            foreach (var ev in Events)
            {
                if (Users.Any(x => x.Id == ev.UserId))
                    continue;

                command.CommandType = System.Data.CommandType.Text;
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@Id", ev.UserId);
                command.CommandText = @"SELECT Id, FirstName, LastName, Email 
                                            FROM AspNetUsers WHERE Id = @Id";

                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var user = new User
                    {
                        Id = reader["Id"].ToString(),
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Email = reader["Email"].ToString()
                    };
                    Users.Add(user);
                }
            }
        }

        return Users;
    }

    [FunctionName("ScheduleReminders")]
    public void Run(
        [TimerTrigger("0 0 8 * * *", RunOnStartup = false)] TimerInfo myTimer, // раз на день о 08:00
        [Queue("email-reminders", Connection = "AzureWebJobsStorage")] ICollector<string> reminderQueue,
        ILogger log)
    {
        log.LogInformation("ScheduleReminders triggered at: {time}", DateTime.UtcNow);

        var events = ExecuteEventQuery();
        if (!events.Any())
        {
            log.LogInformation("No events for tomorrow.");
            return;
        }

        var users = ExecuteUserQuery(events);

        foreach (var ev in events)
        {
            var user = users.FirstOrDefault(x => x.Id == ev.UserId);
            if (user == null)
            {
                log.LogWarning("User with Id {userId} not found for event {eventName}", ev.UserId, ev.Name);
                continue;
            }

            var message = new ReminderMessage
            {
                UserId = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                EventName = ev.Name,
                Category = ev.Category,
                Format = ev.Format,
                StartDate = ev.StartDate,
                EndDate = ev.EndDate,
                Description = ev.Description,
                ReminderType = ev.ReminderType
            };

            var json = JsonSerializer.Serialize(message);
            reminderQueue.Add(json);

            log.LogInformation("Queued reminder for user {email} and event {eventName}", user.Email, ev.Name);
        }
    }
}
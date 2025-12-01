using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Text.Json;


namespace SendEmailWorker;

public class SendEmailWorker
{
    [FunctionName("SendEmailWorker")]
    public void Run(
        [QueueTrigger("email-reminders", Connection = "AzureWebJobsStorage")] string message,
        ILogger log)
    {
        log.LogInformation("SendEmailWorker triggered. Queue message: {msg}", message);

        var reminder = JsonSerializer.Deserialize<ReminderMessage>(message);
        if (reminder == null)
        {
            log.LogError("Failed to deserialize ReminderMessage.");
            return;
        }

        var smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential(
                Environment.GetEnvironmentVariable("SmtpUser"),
                Environment.GetEnvironmentVariable("SmtpPassword")),
            EnableSsl = true,
        };

        var uk = new CultureInfo("uk-UA");

        var start = DateTime.Parse(reminder.StartDate);
        var end = DateTime.Parse(reminder.EndDate);

        var mainDateText = start.ToString("d MMMM yyyy", uk);

        var startLine = start.ToString("dd.MM.yyyy, HH:mm", uk);
        var endLine = end.ToString("dd.MM.yyyy, HH:mm", uk);

        string whenText = reminder.ReminderType switch
        {
            "7Days" => $"через 7 днів – <b>{mainDateText}</b>.",
            "1Day" => $"вже завтра – <b>{mainDateText}</b>.",
            "Today" => $"сьогодні – <b>{mainDateText}</b>.",
            _ => $"<b>{mainDateText}</b>."
        };

        var body = $@"
            <p>Привіт, {reminder.FirstName} {reminder.LastName}!</p>

            <p>
            Це нагадування про те, що подія <b>«{reminder.EventName}»</b> 
            з категорії <b>«{reminder.Category}»</b> відбудеться {whenText}
            </p>

            <p><b>Дата проведення:</b><br/>
            <b>Початок:</b> {startLine}<br/>
            <b>Кінець:</b> {endLine}
            </p>

            <p><b>Формат:</b> {reminder.Format}</p>

            <p><b>Опис події:</b><br/>
            {reminder.Description}
            </p>
        ";

        var mailMessage = new MailMessage
        {
            From = new MailAddress(Environment.GetEnvironmentVariable("SmtpFrom") ?? "no-reply@example.com"),
            Subject = reminder.EventName,
            Body = body,
            IsBodyHtml = true
        };


        mailMessage.To.Add(reminder.Email);

        try
        {
            smtpClient.Send(mailMessage);
            log.LogInformation("Email sent to {email} for event {eventName}", reminder.Email, reminder.EventName);
        }
        catch (Exception ex)
        {
            log.LogError(ex, "Error sending email to {email}", reminder.Email);
            throw;
        }
    }
}
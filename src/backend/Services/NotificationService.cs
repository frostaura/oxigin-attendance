using System;
using System.Threading.Tasks;
using OxiginAttendance.Models;

namespace OxiginAttendance.Services
{
    public interface INotificationService
    {
        Task SendSmsNotificationAsync(Notification notification);
        Task SendEmailNotificationAsync(Notification notification);
        Task<string> GenerateNotificationContentAsync(string template, object data);
    }

    public class NotificationService : INotificationService
    {
        public async Task SendSmsNotificationAsync(Notification notification)
        {
            // Simulate sending SMS notification
            Console.WriteLine($"Sending SMS to {notification.Recipient}: {notification.Content}");
            await Task.CompletedTask;
        }

        public async Task SendEmailNotificationAsync(Notification notification)
        {
            // Simulate sending Email notification
            Console.WriteLine($"Sending Email to {notification.Recipient}: {notification.Content}");
            await Task.CompletedTask;
        }

        public async Task<string> GenerateNotificationContentAsync(string template, object data)
        {
            // Simulate generating notification content based on a template and data
            string content = $"Generated content from template: {template} with data: {data}";
            return await Task.FromResult(content);
        }
    }
}

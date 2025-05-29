using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Logging;
using Oxigin.Attendance.Core.Interfaces.Data;
using Oxigin.Attendance.Shared.Models.Entities;

namespace Oxigin.Attendance.Core.Services.Data
{
    /// <summary>
    /// Data access layer for sending email notifications.
    /// </summary>
    public class EmailNotificationsData : INotificationData
    {
        /// <summary>
        /// Logger instance for this data access layer.
        /// </summary>
        private readonly ILogger<EmailNotificationsData> _logger;

        /// <summary>
        /// Constructor for EmailNotificationsData.
        /// </summary>
        /// <param name="logger">Logger instance.</param>
        public EmailNotificationsData(ILogger<EmailNotificationsData> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Send a notification to a user (placeholder implementation).
        /// </summary>
        /// <param name="recipient">The user entity to receive the notification.</param>
        /// <param name="message">The message to send.</param>
        /// <param name="token">A token allowing cancelling downstream operations.</param>
        public async Task SendNotificationAsync(User recipient, string message, CancellationToken token)
        {
            return; // TODO: Remove after testing. This is just to make testing go faster.

            // Basic SMTP email sending (replace with your SMTP config)
            var smtpHost = "smtp.example.com"; // TODO: Replace with your SMTP server
            var smtpPort = 587; // or 25/465 depending on your server
            var smtpUser = "your-smtp-username"; // TODO: Replace with your SMTP username
            var smtpPass = "your-smtp-password"; // TODO: Replace with your SMTP password
            var fromEmail = "noreply@oxigin.com"; // TODO: Replace with your sender email

            var mail = new MailMessage(fromEmail, recipient.Email)
            {
                Subject = "Oxigin Attendance Notification",
                Body = message
            };

            using var smtp = new SmtpClient(smtpHost, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUser, smtpPass),
                EnableSsl = true
            };

            try
            {
                await smtp.SendMailAsync(mail, token);
                _logger.LogInformation($"[EmailNotificationsData] Email sent to {recipient.Email}: {message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[EmailNotificationsData] Failed to send email to {recipient.Email}");
            }
        }
    }
}

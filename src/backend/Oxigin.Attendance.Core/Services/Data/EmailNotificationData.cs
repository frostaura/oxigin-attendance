using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Oxigin.Attendance.Core.Interfaces.Data;
using Oxigin.Attendance.Shared.Models.Config;
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
        /// SMTP configuration settings.
        /// </summary>
        private readonly SmtpConfig _smtpConfig;

        /// <summary>
        /// Constructor for EmailNotificationsData.
        /// </summary>
        /// <param name="logger">Logger instance.</param>
        /// <param name="smtpConfig">SMTP configuration options.</param>
        public EmailNotificationsData(ILogger<EmailNotificationsData> logger, IOptions<SmtpConfig> smtpConfig)
        {
            _logger = logger;
            _smtpConfig = smtpConfig.Value;
        }

        /// <summary>
        /// Send a notification to a user (placeholder implementation).
        /// </summary>
        /// <param name="recipient">The user entity to receive the notification.</param>
        /// <param name="message">The message to send.</param>
        /// <param name="token">A token allowing cancelling downstream operations.</param>
        public async Task SendNotificationAsync(User recipient, string message, CancellationToken token)
        {
            return;
            // Use SMTP config from injected options
            var smtpHost = _smtpConfig.Host;
            var smtpPort = _smtpConfig.Port;
            var smtpUser = _smtpConfig.User;
            var smtpPass = _smtpConfig.Pass;
            var fromEmail = _smtpConfig.From;

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

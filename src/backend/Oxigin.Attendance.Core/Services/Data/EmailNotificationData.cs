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
            // Placeholder: Log the notification attempt
            _logger.LogInformation($"[EmailNotificationsData] Sending notification to user {recipient?.Id} ({recipient?.Email}): {message}");
            await Task.CompletedTask;
        }
    }
}

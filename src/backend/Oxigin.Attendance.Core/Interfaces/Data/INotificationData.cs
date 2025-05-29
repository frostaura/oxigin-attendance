using Oxigin.Attendance.Shared.Models.Entities;

namespace Oxigin.Attendance.Core.Interfaces.Data
{
    /// <summary>
    /// Interface for notification-related use cases.
    /// </summary>
    public interface INotificationData
    {
        /// <summary>
        /// Send a notification (placeholder).
        /// </summary>
        /// <param name="recipient">The user entity to receive the notification.</param>
        /// <param name="message">The message to send.</param>
        /// <param name="token">Cancellation token.</param>
        Task SendNotificationAsync(User recipient, string message, CancellationToken token);
    }
}

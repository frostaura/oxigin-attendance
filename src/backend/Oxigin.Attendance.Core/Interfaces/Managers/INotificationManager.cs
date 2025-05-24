using Oxigin.Attendance.Shared.Models.Entities;

namespace Oxigin.Attendance.Core.Interfaces.Managers;

public interface INotificationManager
{
    Task<bool> SendReminderAsync(ReminderRequest reminder, CancellationToken token);
    Task<List<NotificationRecord>> GetNotificationHistoryAsync(CancellationToken token);
}

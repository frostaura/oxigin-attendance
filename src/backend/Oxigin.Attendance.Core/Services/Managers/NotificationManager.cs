using Oxigin.Attendance.Core.Interfaces.Managers;
using Oxigin.Attendance.Shared.Models.Entities;

namespace Oxigin.Attendance.Core.Services.Managers;

public class NotificationManager : INotificationManager
{
    public Task<bool> SendReminderAsync(ReminderRequest reminder, CancellationToken token)
    {
        // TODO: Implement logic
        return Task.FromResult(true);
    }
    public Task<List<NotificationRecord>> GetNotificationHistoryAsync(CancellationToken token)
    {
        // TODO: Implement logic
        return Task.FromResult(new List<NotificationRecord>());
    }
}

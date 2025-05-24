// Controller for sending notifications and reminders to users.
using Microsoft.AspNetCore.Mvc;
using Oxigin.Attendance.API.Abstractions;
using Oxigin.Attendance.Core.Interfaces.Managers;
using Oxigin.Attendance.Shared.Models.Entities;

namespace Oxigin.Attendance.API.Controllers;

/// <summary>
/// Provides endpoints for sending reminders and retrieving notification history.
/// </summary>
[ApiController]
[Route("[controller]")]
public class NotificationsController : BaseController
{
    private readonly INotificationManager _notificationManager;

    /// <summary>
    /// Constructor for NotificationsController.
    /// </summary>
    /// <param name="notificationManager">Injected notification manager.</param>
    /// <param name="logger">Logger instance.</param>
    public NotificationsController(INotificationManager notificationManager, ILogger<NotificationsController> logger)
        : base(logger)
    {
        _notificationManager = notificationManager;
    }

    /// <summary>
    /// Send a reminder notification.
    /// </summary>
    [HttpPost("send-reminder")]
    public async Task<IActionResult> SendReminder([FromBody] ReminderRequest reminder, CancellationToken token)
        => Ok(await _notificationManager.SendReminderAsync(reminder, token));

    /// <summary>
    /// Get notification history for the current user.
    /// </summary>
    [HttpGet("history")]
    public async Task<IActionResult> GetNotificationHistory(CancellationToken token)
        => Ok(await _notificationManager.GetNotificationHistoryAsync(token));
}

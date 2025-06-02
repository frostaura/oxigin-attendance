using Oxigin.Attendance.Shared.Models.Entities;

namespace Oxigin.Attendance.Core.Interfaces.Managers;

/// <summary>
/// Manager interface for handling Timesheet operations.
/// </summary>
public interface ITimesheetManager
{
    /// <summary>
    /// Get all timesheets.
    /// </summary>
    /// <param name="token">Cancellation token.</param>
    /// <returns>List of Timesheet entities.</returns>
    Task<IEnumerable<Timesheet>> GetAllAsync(CancellationToken token);
    /// <summary>
    /// Get all timesheets for a specific job.
    /// </summary>
    /// <param name="jobId">The Job ID.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>List of Timesheet entities for the job.</returns>
    Task<IEnumerable<Timesheet>> GetByJobIdAsync(Guid jobId, CancellationToken token);
    /// <summary>
    /// Sign in (create a new timesheet entry).
    /// </summary>
    /// <param name="timesheet">The Timesheet entity to add.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The created Timesheet entity.</returns>
    Task<Timesheet> SignInAsync(Timesheet timesheet, CancellationToken token);
    /// <summary>
    /// Sign out (update the out time of a timesheet entry).
    /// </summary>
    /// <param name="timesheetId">The Timesheet ID.</param>
    /// <param name="outTime">The sign-out time.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The updated Timesheet entity.</returns>
    Task<Timesheet> SignOutAsync(Guid timesheetId, DateTime outTime, CancellationToken token);
}

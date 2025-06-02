using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Oxigin.Attendance.Core.Interfaces.Managers;
using Oxigin.Attendance.Datastore.Interfaces;
using Oxigin.Attendance.Shared.Models.Entities;

namespace Oxigin.Attendance.Core.Services.Managers;

/// <summary>
/// Manager for handling Timesheet operations.
/// </summary>
public class TimesheetManager : ITimesheetManager
{
    /// <summary>
    /// The database context for accessing and persisting timesheets.
    /// </summary>
    private readonly IDatastoreContext _db;
    /// <summary>
    /// Logger instance for this manager.
    /// </summary>
    private readonly ILogger<TimesheetManager> _logger;

    /// <summary>
    /// Constructor for TimesheetManager.
    /// </summary>
    /// <param name="db">The database context.</param>
    /// <param name="logger">Logger instance.</param>
    public TimesheetManager(IDatastoreContext db, ILogger<TimesheetManager> logger)
    {
        _db = db;
        _logger = logger;
    }

    /// <summary>
    /// Get all timesheets, including related Job and Employee entities, omitting deleted entities.
    /// </summary>
    /// <param name="token">Cancellation token.</param>
    /// <returns>List of all non-deleted timesheets.</returns>
    public async Task<IEnumerable<Timesheet>> GetAllAsync(CancellationToken token)
    {
        return await _db.Timesheets
            .Include(t => t.Job)
            .Include(t => t.Employee)
            .Where(t => !t.Deleted && !t.Job.Deleted && !t.Employee.Deleted)
            .ToListAsync(token);
    }

    /// <summary>
    /// Get all timesheets for a specific job, including related Job and Employee entities, omitting deleted entities.
    /// </summary>
    /// <param name="jobId">The Job ID.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>List of timesheets for the specified job, omitting deleted entities.</returns>
    public async Task<IEnumerable<Timesheet>> GetByJobIdAsync(Guid jobId, CancellationToken token)
    {
        return await _db.Timesheets
            .Where(t => t.JobID == jobId && !t.Deleted && !t.Job.Deleted && !t.Employee.Deleted)
            .Include(t => t.Job)
            .Include(t => t.Employee)
            .ToListAsync(token);
    }

    /// <summary>
    /// Sign in (create a new timesheet entry).
    /// </summary>
    /// <param name="timesheet">The Timesheet entity to add.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The created Timesheet entity.</returns>
    public async Task<Timesheet> SignInAsync(Timesheet timesheet, CancellationToken token)
    {
        timesheet.Id = Guid.NewGuid();
        timesheet.TimeIn = timesheet.TimeIn == default ? DateTime.UtcNow : timesheet.TimeIn;
        _db.Timesheets.Add(timesheet);
        await _db.SaveChangesAsync(token);
        return timesheet;
    }

    /// <summary>
    /// Sign out (update the out time of a timesheet entry).
    /// </summary>
    /// <param name="timesheetId">The Timesheet ID.</param>
    /// <param name="outTime">The sign-out time.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The updated Timesheet entity.</returns>
    public async Task<Timesheet> SignOutAsync(Guid timesheetId, DateTime outTime, CancellationToken token)
    {
        var timesheet = await _db.Timesheets.FirstOrDefaultAsync(t => t.Id == timesheetId, token);
        if (timesheet == null) throw new Exception("Timesheet not found");
        timesheet.TimeOut = outTime == default ? DateTime.UtcNow : outTime;
        await _db.SaveChangesAsync(token);
        return timesheet;
    }
}

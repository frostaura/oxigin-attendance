using Microsoft.AspNetCore.Mvc;
using Oxigin.Attendance.API.Abstractions;
using Oxigin.Attendance.Core.Interfaces.Managers;
using Oxigin.Attendance.Datastore.Interfaces;
using Oxigin.Attendance.Shared.Models.Entities;

namespace Oxigin.Attendance.API.Controllers;

/// <summary>
/// Controller for managing Timesheet entities (sign in, sign out, get timesheets).
/// </summary>
[ApiController]
[Route("[controller]")]
public class TimesheetController : BaseController
{
    private readonly ITimesheetManager _timesheetManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="TimesheetController"/> class.
    /// </summary>
    /// <param name="timesheetManager">The timesheet manager service.</param>
    /// <param name="logger">The logger instance.</param>
    /// <param name="db">The database context.</param>
    public TimesheetController(ITimesheetManager timesheetManager, ILogger<TimesheetController> logger, IDatastoreContext db)
        : base(logger, db)
    {
        _timesheetManager = timesheetManager;
    }

    /// <summary>
    /// Get all timesheets.
    /// </summary>
    /// <param name="token">Cancellation token.</param>
    /// <returns>List of Timesheet entities.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken token)
    {
        var signedInUser = await GetRequestingUserAsync(token);
        if (signedInUser == null) return Forbid();
        var timesheets = await _timesheetManager.GetAllAsync(token);
        return Ok(timesheets);
    }

    /// <summary>
    /// Get all timesheets for a particular job.
    /// </summary>
    /// <param name="jobId">The Job ID.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>List of Timesheet entities for the job.</returns>
    [HttpGet("job/{jobId}")]
    public async Task<IActionResult> GetByJobId(Guid jobId, CancellationToken token)
    {
        var signedInUser = await GetRequestingUserAsync(token);
        if (signedInUser == null) return Forbid();
        var timesheets = await _timesheetManager.GetByJobIdAsync(jobId, token);
        return Ok(timesheets);
    }

    /// <summary>
    /// Sign in (create a new timesheet entry).
    /// </summary>
    /// <param name="timesheet">The Timesheet entity to add.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The created Timesheet entity.</returns>
    [HttpPost("signin")]
    public async Task<IActionResult> SignIn([FromBody] Timesheet timesheet, CancellationToken token)
    {
        var signedInUser = await GetRequestingUserAsync(token);
        if (signedInUser == null) return Forbid();
        var created = await _timesheetManager.SignInAsync(timesheet, token);
        return Ok(created);
    }

    /// <summary>
    /// Sign out (update the out time of a timesheet entry).
    /// </summary>
    /// <param name="timesheetId">The Timesheet ID.</param>
    /// <param name="outTime">The sign-out time.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The updated Timesheet entity.</returns>
    [HttpPut("signout/{timesheetId}")]
    public async Task<IActionResult> SignOut(Guid timesheetId, [FromBody] DateTime outTime, CancellationToken token)
    {
        var signedInUser = await GetRequestingUserAsync(token);
        if (signedInUser == null) return Forbid();
        var updated = await _timesheetManager.SignOutAsync(timesheetId, outTime, token);
        return Ok(updated);
    }
}

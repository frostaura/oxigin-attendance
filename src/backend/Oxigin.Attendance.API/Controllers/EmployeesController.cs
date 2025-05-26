// Controller for employee features: authentication, job assignments, and check-ins.
using Microsoft.AspNetCore.Mvc;
using Oxigin.Attendance.API.Abstractions;
using Oxigin.Attendance.Core.Interfaces.Managers;
using Oxigin.Attendance.Shared.Models.Entities;

namespace Oxigin.Attendance.API.Controllers;

/// <summary>
/// Provides endpoints for employee operations: authenticate, assigned jobs, check-in, and history.
/// </summary>
[ApiController]
[Route("[controller]")]
public class EmployeesController : BaseController
{
    private readonly IEmployeeManager _employeeManager;

    /// <summary>
    /// Constructor for EmployeesController.
    /// </summary>
    /// <param name="employeeManager">Injected employee manager.</param>
    /// <param name="logger">Logger instance.</param>
    public EmployeesController(IEmployeeManager employeeManager, ILogger<EmployeesController> logger)
        : base(logger)
    {
        _employeeManager = employeeManager;
    }

    /// <summary>
    /// Authenticate an employee.
    /// </summary>
    [HttpPost("authenticate")]
    public async Task<IActionResult> Authenticate([FromBody] EmployeeLogin login, CancellationToken token)
        => Ok(await _employeeManager.AuthenticateAsync(login, token));

    /// <summary>
    /// Get assigned jobs for the current employee.
    /// </summary>
    [HttpGet("assigned-jobs")]
    public async Task<IActionResult> GetAssignedJobs(CancellationToken token)
        => Ok(await _employeeManager.GetAssignedJobsAsync(token));

    /// <summary>
    /// Check in onsite with GPS and facial recognition data.
    /// </summary>
    [HttpPost("check-in")]
    public async Task<IActionResult> CheckIn([FromBody] CheckInData checkIn, CancellationToken token)
        => Ok(await _employeeManager.CheckInAsync(checkIn, token));

    /// <summary>
    /// Get check-in history for the current employee.
    /// </summary>
    [HttpGet("check-in-history")]
    public async Task<IActionResult> GetCheckInHistory(CancellationToken token)
        => Ok(await _employeeManager.GetCheckInHistoryAsync(token));
}

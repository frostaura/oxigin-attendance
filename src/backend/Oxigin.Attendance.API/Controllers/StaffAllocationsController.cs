// Controller for staff allocation and notification features.
using Microsoft.AspNetCore.Mvc;
using Oxigin.Attendance.API.Abstractions;
using Oxigin.Attendance.Core.Interfaces.Managers;
using Oxigin.Attendance.Shared.Models.Entities;

namespace Oxigin.Attendance.API.Controllers;

/// <summary>
/// Provides endpoints for allocating staff to events and notifying employees.
/// </summary>
[ApiController]
[Route("[controller]")]
public class StaffAllocationsController : BaseController
{
    private readonly IStaffAllocationManager _staffAllocationManager;

    /// <summary>
    /// Constructor for StaffAllocationsController.
    /// </summary>
    /// <param name="staffAllocationManager">Injected staff allocation manager.</param>
    /// <param name="logger">Logger instance.</param>
    public StaffAllocationsController(IStaffAllocationManager staffAllocationManager, ILogger<StaffAllocationsController> logger)
        : base(logger)
    {
        _staffAllocationManager = staffAllocationManager;
    }

    /// <summary>
    /// Allocate staff to an event.
    /// </summary>
    [HttpPost("allocate")]
    public async Task<IActionResult> AllocateStaffToEvent([FromBody] StaffAllocation allocation, CancellationToken token)
        => Ok(await _staffAllocationManager.AllocateStaffToEventAsync(allocation, token));

    /// <summary>
    /// Get allocations for a specific event.
    /// </summary>
    [HttpGet("event/{eventId}")]
    public async Task<IActionResult> GetAllocationsForEvent(Guid eventId, CancellationToken token)
        => Ok(await _staffAllocationManager.GetAllocationsForEventAsync(eventId, token));

    /// <summary>
    /// Notify allocated employees for an event.
    /// </summary>
    [HttpPost("notify")]
    public async Task<IActionResult> NotifyAllocatedEmployees([FromBody] NotificationRequest notification, CancellationToken token)
        => Ok(await _staffAllocationManager.NotifyAllocatedEmployeesAsync(notification, token));
}

// Controller for administrative features such as managing employees, clients, and timesheets.
using Microsoft.AspNetCore.Mvc;
using Oxigin.Attendance.API.Abstractions;
using Oxigin.Attendance.Core.Interfaces.Managers;
using Oxigin.Attendance.Shared.Models.Entities;

namespace Oxigin.Attendance.API.Controllers;

/// <summary>
/// Provides endpoints for admin operations: dashboard, employees, clients, requests, and allocations.
/// </summary>
[ApiController]
[Route("[controller]")]
public class AdminController : BaseController
{
    private readonly IAdminManager _adminManager;

    /// <summary>
    /// Constructor for AdminController.
    /// </summary>
    /// <param name="adminManager">Injected admin manager.</param>
    /// <param name="logger">Logger instance.</param>
    public AdminController(IAdminManager adminManager, ILogger<AdminController> logger)
        : base(logger)
    {
        _adminManager = adminManager;
    }

    /// <summary>
    /// Get dashboard statistics for the admin panel.
    /// </summary>
    [HttpGet("dashboard-stats")]
    public async Task<IActionResult> GetDashboardStats(CancellationToken token)
        => Ok(await _adminManager.GetDashboardStatsAsync(token));

    /// <summary>
    /// Generate a timesheet for a given request.
    /// </summary>
    [HttpPost("generate-timesheet")]
    public async Task<FileResult> GenerateTimesheet([FromBody] TimesheetRequest request, CancellationToken token)
        => File(await _adminManager.GenerateTimesheetAsync(request, token), "application/pdf", "timesheet.pdf");

    /// <summary>
    /// Get all employees.
    /// </summary>
    [HttpGet("employees")]
    public async Task<IActionResult> GetEmployees(CancellationToken token)
        => Ok(await _adminManager.GetEmployeesAsync(token));

    /// <summary>
    /// Create or update an employee.
    /// </summary>
    [HttpPost("employee")]
    public async Task<IActionResult> CreateOrUpdateEmployee([FromBody] Employee employee, CancellationToken token)
        => Ok(await _adminManager.CreateOrUpdateEmployeeAsync(employee, token));

    /// <summary>
    /// Delete an employee by ID.
    /// </summary>
    [HttpDelete("employee/{id}")]
    public async Task<IActionResult> DeleteEmployee(Guid id, CancellationToken token)
        => Ok(await _adminManager.DeleteEmployeeAsync(id, token));

    /// <summary>
    /// Get all clients.
    /// </summary>
    [HttpGet("clients")]
    public async Task<IActionResult> GetClients(CancellationToken token)
        => Ok(await _adminManager.GetClientsAsync(token));

    /// <summary>
    /// Create or update a client.
    /// </summary>
    [HttpPost("client")]
    public async Task<IActionResult> CreateOrUpdateClient([FromBody] Client client, CancellationToken token)
        => Ok(await _adminManager.CreateOrUpdateClientAsync(client, token));

    /// <summary>
    /// Delete a client by ID.
    /// </summary>
    [HttpDelete("client/{id}")]
    public async Task<IActionResult> DeleteClient(Guid id, CancellationToken token)
        => Ok(await _adminManager.DeleteClientAsync(id, token));

    /// <summary>
    /// Get all requests.
    /// </summary>
    [HttpGet("requests")]
    public async Task<IActionResult> GetRequests(CancellationToken token)
        => Ok(await _adminManager.GetRequestsAsync(token));

    /// <summary>
    /// Allocate staff to a client request.
    /// </summary>
    [HttpPost("allocate-staff")]
    public async Task<IActionResult> AllocateStaff([FromBody] StaffAllocation allocation, CancellationToken token)
        => Ok(await _adminManager.AllocateStaffAsync(allocation, token));
}

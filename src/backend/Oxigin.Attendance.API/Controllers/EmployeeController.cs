using Microsoft.AspNetCore.Mvc;
using Oxigin.Attendance.API.Abstractions;
using Oxigin.Attendance.Core.Interfaces.Managers;
using Oxigin.Attendance.Datastore.Interfaces;
using Oxigin.Attendance.Shared.Models.Entities;

namespace Oxigin.Attendance.API.Controllers;

/// <summary>
/// Controller for managing Employee entities.
/// </summary>
[ApiController]
[Route("[controller]")]
public class EmployeeController : BaseController
{
    /// <summary>
    /// The employee manager service for handling employee operations.
    /// </summary>
    private readonly IEmployeeManager _employeeManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmployeeController"/> class.
    /// </summary>
    /// <param name="employeeManager">The employee manager service.</param>
    /// <param name="logger">The logger instance.</param>
    /// <param name="db">The database context.</param>
    public EmployeeController(IEmployeeManager employeeManager, ILogger<EmployeeController> logger, IDatastoreContext db)
        : base(logger, db)
    {
        _employeeManager = employeeManager;
    }

    /// <summary>
    /// Get all employees.
    /// </summary>
    /// <param name="token">Cancellation token.</param>
    /// <returns>List of Employee entities.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken token)
    {
        var signedInUser = await GetRequestingUserAsync(token);
        if (signedInUser == null) return Forbid();
        var employees = await _employeeManager.GetAllAsync(token);
        return Ok(employees);
    }

    /// <summary>
    /// Get an employee by ID.
    /// </summary>
    /// <param name="id">The Employee ID.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The Employee entity, or NotFound if not found.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken token)
    {
        var signedInUser = await GetRequestingUserAsync(token);
        if (signedInUser == null) return Forbid();
        var employee = await _employeeManager.GetByIdAsync(id, token);
        if (employee == null) return NotFound();
        return Ok(employee);
    }

    /// <summary>
    /// Add a new employee.
    /// </summary>
    /// <param name="employee">The Employee entity to add.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The created Employee entity.</returns>
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] Employee employee, CancellationToken token)
    {
        var signedInUser = await GetRequestingUserAsync(token);
        if (signedInUser == null) return Forbid();
        var created = await _employeeManager.AddAsync(employee, token);
        return Ok(created);
    }

    /// <summary>
    /// Update an existing employee.
    /// </summary>
    /// <param name="employee">The Employee entity with updated details.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The updated Employee entity.</returns>
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] Employee employee, CancellationToken token)
    {
        var signedInUser = await GetRequestingUserAsync(token);
        if (signedInUser == null) return Forbid();
        var updated = await _employeeManager.UpdateAsync(employee, token);
        return Ok(updated);
    }

    /// <summary>
    /// Remove an employee by ID.
    /// </summary>
    /// <param name="id">The Employee ID.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>No content if successful.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Remove(Guid id, CancellationToken token)
    {
        var signedInUser = await GetRequestingUserAsync(token);
        if (signedInUser == null) return Forbid();
        await _employeeManager.RemoveAsync(id, token);
        return NoContent();
    }
}

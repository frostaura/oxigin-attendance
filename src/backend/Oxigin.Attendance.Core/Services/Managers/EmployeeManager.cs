using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Oxigin.Attendance.Core.Interfaces.Managers;
using Oxigin.Attendance.Datastore.Interfaces;
using Oxigin.Attendance.Shared.Models.Entities;

namespace Oxigin.Attendance.Core.Services.Managers;

/// <summary>
/// Manager for handling Employee CRUD operations.
/// </summary>
public class EmployeeManager : IEmployeeManager
{
    /// <summary>
    /// The database context for accessing and persisting employees.
    /// </summary>
    private readonly IDatastoreContext _db;
    /// <summary>
    /// Logger instance for this manager.
    /// </summary>
    private readonly ILogger<EmployeeManager> _logger;

    /// <summary>
    /// Constructor for EmployeeManager.
    /// </summary>
    /// <param name="db">The database context.</param>
    /// <param name="logger">Logger instance.</param>
    public EmployeeManager(IDatastoreContext db, ILogger<EmployeeManager> logger)
    {
        _db = db;
        _logger = logger;
    }

    /// <summary>
    /// Get all employees.
    /// </summary>
    /// <param name="token">Cancellation token.</param>
    /// <returns>List of Employee entities.</returns>
    public async Task<IEnumerable<Employee>> GetAllAsync(CancellationToken token)
    {
        return await _db.Employees.Where(e => !e.Deleted).ToListAsync(token);
    }

    /// <summary>
    /// Get an employee by ID.
    /// </summary>
    /// <param name="id">The Employee ID.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The Employee entity, or null if not found.</returns>
    public async Task<Employee?> GetByIdAsync(Guid id, CancellationToken token)
    {
        return await _db.Employees.FirstOrDefaultAsync(e => e.Id == id && !e.Deleted, token);
    }

    /// <summary>
    /// Add a new employee.
    /// </summary>
    /// <param name="employee">The Employee entity to add.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The created Employee entity.</returns>
    public async Task<Employee> AddAsync(Employee employee, CancellationToken token)
    {
        _db.Employees.Add(employee);
        await _db.SaveChangesAsync(token);
        return employee;
    }

    /// <summary>
    /// Update an existing employee.
    /// </summary>
    /// <param name="employee">The Employee entity with updated details.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The updated Employee entity.</returns>
    public async Task<Employee> UpdateAsync(Employee employee, CancellationToken token)
    {
        _db.Employees.Update(employee);
        await _db.SaveChangesAsync(token);
        return employee;
    }

    /// <summary>
    /// Remove an employee by ID.
    /// </summary>
    /// <param name="id">The Employee ID.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task RemoveAsync(Guid id, CancellationToken token)
    {
        var employee = await _db.Employees.FirstOrDefaultAsync(e => e.Id == id, token);
        if (employee != null)
        {
            _db.Employees.Remove(employee);
            await _db.SaveChangesAsync(token);
        }
    }
}

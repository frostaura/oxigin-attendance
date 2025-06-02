using Oxigin.Attendance.Shared.Models.Entities;

namespace Oxigin.Attendance.Core.Interfaces.Managers;

/// <summary>
/// Manager interface for handling Employee CRUD operations.
/// </summary>
public interface IEmployeeManager
{
    /// <summary>
    /// Get all employees.
    /// </summary>
    /// <param name="token">Cancellation token.</param>
    /// <returns>List of Employee entities.</returns>
    Task<IEnumerable<Employee>> GetAllAsync(CancellationToken token);

    /// <summary>
    /// Get an employee by ID.
    /// </summary>
    /// <param name="id">The Employee ID.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The Employee entity, or null if not found.</returns>
    Task<Employee?> GetByIdAsync(Guid id, CancellationToken token);

    /// <summary>
    /// Add a new employee.
    /// </summary>
    /// <param name="employee">The Employee entity to add.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The created Employee entity.</returns>
    Task<Employee> AddAsync(Employee employee, CancellationToken token);

    /// <summary>
    /// Update an existing employee.
    /// </summary>
    /// <param name="employee">The Employee entity with updated details.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The updated Employee entity.</returns>
    Task<Employee> UpdateAsync(Employee employee, CancellationToken token);

    /// <summary>
    /// Remove an employee by ID.
    /// </summary>
    /// <param name="id">The Employee ID.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task RemoveAsync(Guid id, CancellationToken token);
}

using Oxigin.Attendance.Shared.Models.Entities;

namespace Oxigin.Attendance.Core.Interfaces.Managers;

/// <summary>
/// Manager interface for handling job allocation operations.
/// </summary>
public interface IJobAllocationManager
{
    /// <summary>
    /// Get all allocations for a given job.
    /// </summary>
    /// <param name="jobId">The job ID.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>List of JobAllocation entities.</returns>
    Task<IEnumerable<Allocation>> GetAllocationsForJobAsync(Guid jobId, CancellationToken token);

    /// <summary>
    /// Get all allocations for a given employee.
    /// </summary>
    /// <param name="employeeId">The employee ID.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>List of JobAllocation entities.</returns>
    Task<IEnumerable<Allocation>> GetAllocationsForEmployeeAsync(Guid employeeId, CancellationToken token);

    /// <summary>
    /// Create a new allocation for a job.
    /// </summary>
    /// <param name="allocation">The JobAllocation entity to create.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The created JobAllocation entity.</returns>
    Task<Allocation> CreateAllocationAsync(Allocation allocation, CancellationToken token);

    /// <summary>
    /// Remove an allocation by ID.
    /// </summary>
    /// <param name="id">The JobAllocation ID.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task RemoveAllocationAsync(Guid id, CancellationToken token);

    /// <summary>
    /// Get an allocation by ID.
    /// </summary>
    /// <param name="id">The JobAllocation ID.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The JobAllocation entity, or null if not found.</returns>
    Task<Allocation?> GetByIdAsync(Guid id, CancellationToken token);
}

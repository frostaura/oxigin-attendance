using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Oxigin.Attendance.Core.Interfaces.Managers;
using Oxigin.Attendance.Datastore.Interfaces;
using Oxigin.Attendance.Shared.Models.Entities;

namespace Oxigin.Attendance.Core.Services.Managers;

/// <summary>
/// Manager for handling job allocation operations.
/// </summary>
public class JobAllocationManager : IJobAllocationManager
{
    /// <summary>
    /// The database context for accessing and persisting job allocations.
    /// </summary>
    private readonly IDatastoreContext _db;
    /// <summary>
    /// Logger instance for this manager.
    /// </summary>
    private readonly ILogger<JobAllocationManager> _logger;

    /// <summary>
    /// Constructor for JobAllocationManager.
    /// </summary>
    /// <param name="db">The database context.</param>
    /// <param name="logger">Logger instance.</param>
    public JobAllocationManager(IDatastoreContext db, ILogger<JobAllocationManager> logger)
    {
        _db = db;
        _logger = logger;
    }

    /// <summary>
    /// Get all allocations for a given job.
    /// </summary>
    /// <param name="jobId">The job ID.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>List of Allocation entities.</returns>
    public async Task<IEnumerable<Allocation>> GetAllocationsForJobAsync(Guid jobId, CancellationToken token)
    {
        return await _db.Allocations
            .Include(a => a.Job)
            .Include(a => a.Employee)
            .Where(a => a.JobID == jobId && !a.Deleted)
            .ToListAsync(token);
    }

    /// <summary>
    /// Get all allocations for a given employee.
    /// </summary>
    /// <param name="employeeId">The employee ID.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>List of Allocation entities.</returns>
    public async Task<IEnumerable<Allocation>> GetAllocationsForEmployeeAsync(Guid employeeId, CancellationToken token)
    {
        return await _db.Allocations
            .Include(a => a.Job)
            .Include(a => a.Employee)
            .Where(a => a.EmployeeID == employeeId && !a.Deleted)
            .ToListAsync(token);
    }

    /// <summary>
    /// Create a new allocation for a job.
    /// </summary>
    /// <param name="allocation">The Allocation entity to create.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The created Allocation entity.</returns>
    public async Task<Allocation> CreateAllocationAsync(Allocation allocation, CancellationToken token)
    {
        // Attach existing Job and Employee instead of trying to create new ones
        var job = await _db.Jobs.FindAsync(new object[] { allocation.JobID }, token);
        var employee = await _db.Employees.FindAsync(new object[] { allocation.EmployeeID }, token);

        if (job == null || employee == null)
        {
            throw new InvalidOperationException("Job or Employee not found");
        }

        // Create new allocation with references to existing entities
        var newAllocation = new Allocation
        {
            Name = allocation.Name,
            Description = allocation.Description,
            Time = allocation.Time,
            HoursNeeded = allocation.HoursNeeded,
            JobID = allocation.JobID,
            EmployeeID = allocation.EmployeeID,
            Deleted = false
        };

        _db.Allocations.Add(newAllocation);
        await _db.SaveChangesAsync(token);
        return newAllocation;
    }

    /// <summary>
    /// Remove an allocation by ID (soft delete).
    /// </summary>
    /// <param name="id">The Allocation ID.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task RemoveAllocationAsync(Guid id, CancellationToken token)
    {
        var allocation = await _db.Allocations.FirstOrDefaultAsync(a => a.Id == id && !a.Deleted, token);
        if (allocation != null)
        {
            allocation.Deleted = true;
            await _db.SaveChangesAsync(token);
        }
    }

    /// <summary>
    /// Get an allocation by ID.
    /// </summary>
    /// <param name="id">The Allocation ID.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The Allocation entity, or null if not found.</returns>
    public async Task<Allocation?> GetByIdAsync(Guid id, CancellationToken token)
    {
        return await _db.Allocations
            .Include(a => a.Job)
            .Include(a => a.Employee)
            .FirstOrDefaultAsync(a => a.Id == id && !a.Deleted, token);
    }
}

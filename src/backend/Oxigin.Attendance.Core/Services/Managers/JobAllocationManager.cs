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
        return await _db.Allocations.Where(a => a.JobID == jobId && !a.Deleted).ToListAsync(token);
    }

    /// <summary>
    /// Create a new allocation for a job.
    /// </summary>
    /// <param name="allocation">The Allocation entity to create.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The created Allocation entity.</returns>
    public async Task<Allocation> CreateAllocationAsync(Allocation allocation, CancellationToken token)
    {
        _db.Allocations.Add(allocation);
        await _db.SaveChangesAsync(token);
        return allocation;
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
        return await _db.Allocations.FirstOrDefaultAsync(a => a.Id == id && !a.Deleted, token);
    }
}

using Microsoft.AspNetCore.Mvc;
using Oxigin.Attendance.API.Abstractions;
using Oxigin.Attendance.Core.Interfaces.Managers;
using Oxigin.Attendance.Datastore.Interfaces;
using Oxigin.Attendance.Shared.Models.Entities;

namespace Oxigin.Attendance.API.Controllers;

/// <summary>
/// Controller for managing job allocations.
/// </summary>
[ApiController]
[Route("[controller]")]
public class JobAllocationController : BaseController
{
    /// <summary>
    /// The job allocation manager service for handling allocation operations.
    /// </summary>
    private readonly IJobAllocationManager _jobAllocationManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="JobAllocationController"/> class.
    /// </summary>
    /// <param name="jobAllocationManager">The job allocation manager service.</param>
    /// <param name="logger">The logger instance.</param>
    /// <param name="db">The database context.</param>
    public JobAllocationController(IJobAllocationManager jobAllocationManager, ILogger<JobAllocationController> logger, IDatastoreContext db)
        : base(logger, db)
    {
        _jobAllocationManager = jobAllocationManager;
    }

    /// <summary>
    /// Get all allocations for a given job.
    /// </summary>
    /// <param name="jobId">The job ID.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>List of JobAllocation entities.</returns>
    [HttpGet("job/{jobId}")]
    public async Task<IActionResult> GetAllocationsForJob(Guid jobId, CancellationToken token)
    {
        var signedInUser = await GetRequestingUserAsync(token);
        if (signedInUser == null) return Forbid();
        var allocations = await _jobAllocationManager.GetAllocationsForJobAsync(jobId, token);
        return Ok(allocations.Where(a => !a.Deleted));
    }

    /// <summary>
    /// Create a new allocation for a job.
    /// </summary>
    /// <param name="allocation">The JobAllocation entity to create.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The created JobAllocation entity.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateAllocation([FromBody] Allocation allocation, CancellationToken token)
    {
        var signedInUser = await GetRequestingUserAsync(token);
        if (signedInUser == null) return Forbid();
        var created = await _jobAllocationManager.CreateAllocationAsync(allocation, token);
        return Ok(created);
    }

    /// <summary>
    /// Remove an allocation by ID.
    /// </summary>
    /// <param name="id">The JobAllocation ID.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>No content if successful.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveAllocation(Guid id, CancellationToken token)
    {
        var signedInUser = await GetRequestingUserAsync(token);
        if (signedInUser == null) return Forbid();
        await _jobAllocationManager.RemoveAllocationAsync(id, token);
        return NoContent();
    }

    /// <summary>
    /// Get an allocation by ID.
    /// </summary>
    /// <param name="id">The JobAllocation ID.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The JobAllocation entity if found and not deleted, otherwise NotFound.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken token)
    {
        var signedInUser = await GetRequestingUserAsync(token);
        if (signedInUser == null) return Forbid();
        var allocation = await _jobAllocationManager.GetByIdAsync(id, token);
        if (allocation == null || allocation.Deleted)
            return NotFound();
        return Ok(allocation);
    }
}

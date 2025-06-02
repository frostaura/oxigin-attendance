using Microsoft.AspNetCore.Mvc;
using Oxigin.Attendance.API.Abstractions;
using Oxigin.Attendance.Core.Interfaces.Managers;
using Oxigin.Attendance.Datastore.Interfaces;
using Oxigin.Attendance.Shared.Models.Entities;
using System.Threading;

namespace Oxigin.Attendance.API.Controllers;

/// <summary>
/// Controller for managing AdditionalWorker entities linked to jobs.
/// </summary>
[ApiController]
[Route("[controller]")]
public class AdditionalWorkerController : BaseController
{
    private readonly IAdditionalWorkerManager _additionalWorkerManager;

    public AdditionalWorkerController(IAdditionalWorkerManager additionalWorkerManager, ILogger<AdditionalWorkerController> logger, IDatastoreContext db)
        : base(logger, db)
    {
        _additionalWorkerManager = additionalWorkerManager;
    }

    /// <summary>
    /// Get all additional workers for a given job.
    /// </summary>
    /// <param name="jobId">The job ID.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>List of AdditionalWorker entities.</returns>
    [HttpGet("job/{jobId}")]
    public async Task<IActionResult> GetByJobId(Guid jobId, CancellationToken token)
    {
        var workers = await _additionalWorkerManager.GetByJobIdAsync(jobId, token);
        return Ok(workers.Where(w => !w.Deleted));
    }

    /// <summary>
    /// Add an additional worker to a job.
    /// </summary>
    /// <param name="worker">The AdditionalWorker entity (must include JobID).</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The created AdditionalWorker entity.</returns>
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] AdditionalWorker worker, CancellationToken token)
    {
        var created = await _additionalWorkerManager.AddAsync(worker, token);
        return Ok(created);
    }

    /// <summary>
    /// Remove an additional worker from a job.
    /// </summary>
    /// <param name="id">The AdditionalWorker ID.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>No content if successful.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Remove(Guid id, CancellationToken token)
    {
        await _additionalWorkerManager.RemoveAsync(id, token);
        return NoContent();
    }

    /// <summary>
    /// Get all AdditionalWorker entities.
    /// </summary>
    /// <param name="token">Cancellation token.</param>
    /// <returns>List of AdditionalWorker entities.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken token)
    {
        var workers = await _additionalWorkerManager.GetAllAsync(token);
        return Ok(workers.Where(w => !w.Deleted));
    }

    /// <summary>
    /// Get an AdditionalWorker entity by its ID.
    /// </summary>
    /// <param name="id">The AdditionalWorker ID.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The AdditionalWorker entity, or NotFound if it doesn't exist or is soft-deleted.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken token)
    {
        var worker = await _additionalWorkerManager.GetByIdAsync(id, token);
        if (worker == null || worker.Deleted)
            return NotFound();
        return Ok(worker);
    }
}

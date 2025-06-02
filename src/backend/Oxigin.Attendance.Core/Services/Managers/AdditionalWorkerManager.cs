using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Oxigin.Attendance.Core.Interfaces.Managers;
using Oxigin.Attendance.Datastore.Interfaces;
using Oxigin.Attendance.Shared.Models.Entities;

namespace Oxigin.Attendance.Core.Services.Managers;

/// <summary>
/// Manager for handling AdditionalWorker use cases.
/// </summary>
public class AdditionalWorkerManager : IAdditionalWorkerManager
{
    /// <summary>
    /// The database context for accessing and persisting additional workers.
    /// </summary>
    private readonly IDatastoreContext _db;
    /// <summary>
    /// Logger instance for this manager.
    /// </summary>
    private readonly ILogger<AdditionalWorkerManager> _logger;

    /// <summary>
    /// Constructor for AdditionalWorkerManager.
    /// </summary>
    /// <param name="db">The database context.</param>
    /// <param name="logger">Logger instance.</param>
    public AdditionalWorkerManager(IDatastoreContext db, ILogger<AdditionalWorkerManager> logger)
    {
        _db = db;
        _logger = logger;
    }

    /// <summary>
    /// Get all additional workers for a given job.
    /// </summary>
    /// <param name="jobId">The job ID.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>List of AdditionalWorker entities.</returns>
    public async Task<IEnumerable<AdditionalWorker>> GetByJobIdAsync(Guid jobId, CancellationToken token)
    {
        return await _db.AdditionalWorkers.Where(w => w.JobID == jobId).ToListAsync(token);
    }

    /// <summary>
    /// Add an additional worker to a job.
    /// </summary>
    /// <param name="worker">The AdditionalWorker entity (must include JobID).</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The created AdditionalWorker entity.</returns>
    public async Task<AdditionalWorker> AddAsync(AdditionalWorker worker, CancellationToken token)
    {
        _db.AdditionalWorkers.Add(worker);
        await _db.SaveChangesAsync(token);
        return worker;
    }

    /// <summary>
    /// Remove an additional worker from a job.
    /// </summary>
    /// <param name="id">The AdditionalWorker ID.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task RemoveAsync(Guid id, CancellationToken token)
    {
        var worker = await _db.AdditionalWorkers.FirstOrDefaultAsync(w => w.Id == id, token);
        if (worker != null)
        {
            _db.AdditionalWorkers.Remove(worker);
            await _db.SaveChangesAsync(token);
        }
    }
}

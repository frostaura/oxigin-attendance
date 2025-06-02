using Oxigin.Attendance.Shared.Models.Entities;

namespace Oxigin.Attendance.Core.Interfaces.Managers;

/// <summary>
/// Manager interface for handling AdditionalWorker use cases.
/// </summary>
public interface IAdditionalWorkerManager
{
    /// <summary>
    /// Get all additional workers for a given job.
    /// </summary>
    /// <param name="jobId">The job ID.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>List of AdditionalWorker entities.</returns>
    Task<IEnumerable<AdditionalWorker>> GetByJobIdAsync(Guid jobId, CancellationToken token);

    /// <summary>
    /// Add an additional worker to a job.
    /// </summary>
    /// <param name="worker">The AdditionalWorker entity (must include JobID).</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The created AdditionalWorker entity.</returns>
    Task<AdditionalWorker> AddAsync(AdditionalWorker worker, CancellationToken token);

    /// <summary>
    /// Remove an additional worker from a job.
    /// </summary>
    /// <param name="id">The AdditionalWorker ID.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task RemoveAsync(Guid id, CancellationToken token);

    /// <summary>
    /// Get all additional workers.
    /// </summary>
    /// <param name="token">Cancellation token.</param>
    /// <returns>List of AdditionalWorker entities.</returns>
    Task<IEnumerable<AdditionalWorker>> GetAllAsync(CancellationToken token);

    /// <summary>
    /// Get an additional worker by its ID.
    /// </summary>
    /// <param name="id">The AdditionalWorker ID.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The AdditionalWorker entity, if found.</returns>
    Task<AdditionalWorker?> GetByIdAsync(Guid id, CancellationToken token);
}

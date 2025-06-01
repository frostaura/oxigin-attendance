using Microsoft.Extensions.Logging;
using Oxigin.Attendance.Core.Interfaces.Managers;
using Oxigin.Attendance.Shared.Models.Entities;
using Oxigin.Attendance.Datastore.Interfaces;
using Microsoft.EntityFrameworkCore;
using Oxigin.Attendance.Shared.Enums;

namespace Oxigin.Attendance.Core.Services.Managers;

/// <summary>
/// Implementation of client-related use cases (job requests, approvals, etc).
/// </summary>
public class JobRequestManager : IJobRequestManager
{
    /// <summary>
    /// Logger instance for this manager.
    /// </summary>
    private readonly ILogger<JobRequestManager> _logger;
    /// <summary>
    /// The database context for accessing and persisting job requests and related entities.
    /// </summary>
    private readonly IDatastoreContext _db;

    /// <summary>
    /// Constructor for ClientsManager.
    /// </summary>
    /// <param name="db"></param>
    /// <param name="logger">Logger instance.</param>
    public JobRequestManager(IDatastoreContext db, ILogger<JobRequestManager> logger)
    {
        _db = db;
        _logger = logger;
    }

    /// <summary>
    /// Retrieve all job requests for the current client or context.
    /// </summary>
    /// <param name="token">A token for cancelling downstream operations.</param>
    /// <returns>A collection of job requests.</returns>
    public async Task<IEnumerable<JobRequest>> GetJobRequestsAsync(CancellationToken token)
    {
        try
        {
            return await _db.JobRequests.Include(j => j.Client).ToListAsync(token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve job requests");
            throw;
        }
    }

    /// <summary>
    /// Create a new job request for a client, specifying event details and requirements.
    /// </summary>
    /// <param name="request">The job request entity containing event details, staff requirements, etc.</param>
    /// <param name="token">A token for cancelling downstream operations.</param>
    /// <returns>The created job request entity.</returns>
    public async Task<JobRequest> CreateJobRequestAsync(JobRequest request, CancellationToken token)
    {
        try
        {
            request.Status = JobRequestStatus.Pending;
            _db.JobRequests.Add(request);
            await _db.SaveChangesAsync(token);
            return request;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create job request");
            throw;
        }
    }

    /// <summary>
    /// Approve a pending job request, typically by a client or site manager.
    /// </summary>
    /// <param name="request">The job request entity to approve (should include the id and any relevant context).</param>
    /// <param name="token">A token for cancelling downstream operations.</param>
    /// <returns>The updated job request entity with approved status.</returns>
    public async Task<JobRequest> ApproveJobRequestAsync(JobRequest request, CancellationToken token)
    {
        try
        {
            var entity = await _db.JobRequests.FirstOrDefaultAsync(j => j.Id == request.Id, token);
            if (entity == null) throw new InvalidOperationException("Job request not found");
            entity.Status = JobRequestStatus.Approved;
            await _db.SaveChangesAsync(token);
            return entity;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to approve job request");
            throw;
        }
    }

    /// <summary>
    /// Reject a pending job request, typically by a client or site manager.
    /// </summary>
    /// <param name="request">The job request entity to reject (should include the id and any relevant context).</param>
    /// <param name="token">A token for cancelling downstream operations.</param>
    /// <returns>The updated job request entity with rejected status.</returns>
    public async Task<JobRequest> RejectJobRequestAsync(JobRequest request, CancellationToken token)
    {
        try
        {
            var entity = await _db.JobRequests.FirstOrDefaultAsync(j => j.Id == request.Id, token);
            if (entity == null) throw new InvalidOperationException("Job request not found");
            entity.Status = JobRequestStatus.Rejected;
            await _db.SaveChangesAsync(token);
            return entity;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to reject job request");
            throw;
        }
    }
}

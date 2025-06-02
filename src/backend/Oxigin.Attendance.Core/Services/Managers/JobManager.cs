using Microsoft.Extensions.Logging;
using Oxigin.Attendance.Core.Interfaces.Managers;
using Oxigin.Attendance.Shared.Models.Entities;
using Oxigin.Attendance.Datastore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Oxigin.Attendance.Core.Services.Managers;

/// <summary>
/// Implementation of client-related use cases (job requests, approvals, etc).
/// </summary>
public class JobManager : IJobRequestManager
{
    /// <summary>
    /// Logger instance for this manager.
    /// </summary>
    private readonly ILogger<JobManager> _logger;
    /// <summary>
    /// The database context for accessing and persisting job requests and related entities.
    /// </summary>
    private readonly IDatastoreContext _db;

    /// <summary>
    /// Constructor for ClientsManager.
    /// </summary>
    /// <param name="db"></param>
    /// <param name="logger">Logger instance.</param>
    public JobManager(IDatastoreContext db, ILogger<JobManager> logger)
    {
        _db = db;
        _logger = logger;
    }

    /// <summary>
    /// Retrieve all job requests for the current client or context.
    /// </summary>
    /// <param name="token">A token for cancelling downstream operations.</param>
    /// <returns>A collection of job requests.</returns>
    public async Task<IEnumerable<Job>> GetJobRequestsAsync(CancellationToken token)
    {
        try
        {
            return await _db.Jobs.ToListAsync(token);
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
    public async Task<Job> CreateJobRequestAsync(Job request, CancellationToken token)
    {
        try
        {
            request.Approved = false;
            _db.Jobs.Add(request);
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
    public async Task<Job> ApproveJobRequestAsync(Job request, CancellationToken token)
    {
        try
        {
            var entity = await _db.Jobs.FirstOrDefaultAsync(j => j.Id == request.Id, token);
            if (entity == null) throw new InvalidOperationException("Job request not found");
            entity.Approved = true;
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
    public async Task<Job> RejectJobRequestAsync(Job request, CancellationToken token)
    {
        try
        {
            var entity = await _db.Jobs.FirstOrDefaultAsync(j => j.Id == request.Id, token);
            if (entity == null) throw new InvalidOperationException("Job request not found");
            entity.Approved = false;
            await _db.SaveChangesAsync(token);
            return entity;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to reject job request");
            throw;
        }
    }

    /// <summary>
    /// Get all jobs that require approval by the given user (e.g., site manager or client).
    /// </summary>
    /// <param name="user">The user for whom to find jobs requiring approval.</param>
    /// <param name="token">A token for cancelling downstream operations.</param>
    /// <returns>A collection of jobs requiring approval.</returns>
    public async Task<IEnumerable<Job>> GetJobsRequiringApprovalAsync(User user, CancellationToken token)
    {
        // Example: jobs where Approved is false and user is the client or site manager
        return await _db.Jobs.Where(j => !j.Approved && (j.ClientID != user.client.Id || j.RequestorID == user.Id)).ToListAsync(token);
    }

    /// <summary>
    /// Get all jobs that are awaiting confirmation by the given user (e.g., worker or client).
    /// </summary>
    /// <param name="user">The user for whom to find jobs awaiting confirmation.</param>
    /// <param name="token">A token for cancelling downstream operations.</param>
    /// <returns>A collection of jobs awaiting confirmation.</returns>
    public async Task<IEnumerable<Job>> GetJobsAwaitingConfirmationAsync(User user, CancellationToken token)
    {
        // Example: jobs where Approved is true but some other confirmation is needed (customize as needed)
        return await _db.Jobs.Where(j => j.Approved && (j.ClientID == user.Id || j.RequestorID == user.Id)).ToListAsync(token);
    }
}

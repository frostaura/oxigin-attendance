using Oxigin.Attendance.Shared.Models.Entities;

namespace Oxigin.Attendance.Core.Interfaces.Managers;

/// <summary>
/// Interface for client-related use cases (job requests, approvals, etc).
/// </summary>
public interface IJobRequestManager
{
    /// <summary>
    /// Retrieve all job requests for the current client or context.
    /// </summary>
    /// <param name="token">A token for cancelling downstream operations.</param>
    /// <returns>A collection of job requests.</returns>
    Task<IEnumerable<Job>> GetJobRequestsAsync(CancellationToken token);
    /// <summary>
    /// Create a new job request for a client, specifying event details and requirements.
    /// </summary>
    /// <param name="request">The job request entity containing event details, staff requirements, etc.</param>
    /// <param name="token">A token for cancelling downstream operations.</param>
    /// <returns>The created job request entity.</returns>
    Task<Job> CreateJobRequestAsync(Job request, CancellationToken token);
    /// <summary>
    /// Approve a pending job request, typically by a client or site manager.
    /// </summary>
    /// <param name="request">The job request entity to approve (should include the id and any relevant context).</param>
    /// <param name="token">A token for cancelling downstream operations.</param>
    /// <returns>The updated job request entity with approved status.</returns>
    Task<Job> ApproveJobRequestAsync(Job request, CancellationToken token);
    /// <summary>
    /// Reject a pending job request, typically by a client or site manager.
    /// </summary>
    /// <param name="request">The job request entity to reject (should include the id and any relevant context).</param>
    /// <param name="token">A token for cancelling downstream operations.</param>
    /// <returns>The updated job request entity with rejected status.</returns>
    Task<Job> RejectJobRequestAsync(Job request, CancellationToken token);
    /// <summary>
    /// Get all jobs that require approval by the given user (e.g., site manager or client).
    /// </summary>
    /// <param name="user">The user for whom to find jobs requiring approval.</param>
    /// <param name="token">A token for cancelling downstream operations.</param>
    /// <returns>A collection of jobs requiring approval.</returns>
    Task<IEnumerable<Job>> GetJobsRequiringApprovalAsync(User user, CancellationToken token);

    /// <summary>
    /// Get all jobs that are awaiting confirmation by the given user (e.g., worker or client).
    /// </summary>
    /// <param name="user">The user for whom to find jobs awaiting confirmation.</param>
    /// <param name="token">A token for cancelling downstream operations.</param>
    /// <returns>A collection of jobs awaiting confirmation.</returns>
    Task<IEnumerable<Job>> GetJobsAwaitingConfirmationAsync(User user, CancellationToken token);
}

// Interface for managing client requests, including creation, approval, and retrieval.
using Oxigin.Attendance.Shared.Models.Entities;

namespace Oxigin.Attendance.Core.Interfaces.Managers;

// Defines contract for client request management operations.
public interface IClientRequestManager
{
    /// <summary>
    /// Create a new client request.
    /// </summary>
    /// <param name="request">The client request to create.</param>
    /// <param name="token">Cancellation token for async operation.</param>
    /// <returns>Action result indicating success or failure.</returns>
    Task<ClientRequest> CreateRequestAsync(ClientRequest request, CancellationToken token);

    /// <summary>
    /// Get all requests for the current client.
    /// </summary>
    /// <param name="token">Cancellation token for async operation.</param>
    /// <returns>List of client requests.</returns>
    Task<List<ClientRequest>> GetRequestsForClientAsync(CancellationToken token);

    /// <summary>
    /// Approve a client request by its ID.
    /// </summary>
    /// <param name="id">The request ID.</param>
    /// <param name="token">Cancellation token for async operation.</param>
    /// <returns>Action result indicating success or failure.</returns>
    Task<ClientRequest> ApproveRequestAsync(Guid id, CancellationToken token);

    /// <summary>
    /// Reject a client request by its ID.
    /// </summary>
    /// <param name="id">The request ID.</param>
    /// <param name="token">Cancellation token for async operation.</param>
    /// <returns>Action result indicating success or failure.</returns>
    Task<ClientRequest> RejectRequestAsync(Guid id, CancellationToken token);

    /// <summary>
    /// Get all pending approvals for the current user.
    /// </summary>
    /// <param name="token">Cancellation token for async operation.</param>
    /// <returns>List of pending client requests.</returns>
    Task<List<ClientRequest>> GetPendingApprovalsAsync(CancellationToken token);

    /// <summary>
    /// Approve a client request as a manager by its ID.
    /// </summary>
    /// <param name="id">The request ID.</param>
    /// <param name="token">Cancellation token for async operation.</param>
    /// <returns>Action result indicating success or failure.</returns>
    Task<ClientRequest> ApproveRequestByManagerAsync(Guid id, CancellationToken token);

    /// <summary>
    /// Reject a client request as a manager by its ID.
    /// </summary>
    /// <param name="id">The request ID.</param>
    /// <param name="token">Cancellation token for async operation.</param>
    /// <returns>Action result indicating success or failure.</returns>
    Task<ClientRequest> RejectRequestByManagerAsync(Guid id, CancellationToken token);
}

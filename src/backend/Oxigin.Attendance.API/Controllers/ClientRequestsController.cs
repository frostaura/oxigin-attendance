// Controller for client request management: creation, approval, and retrieval.
using Microsoft.AspNetCore.Mvc;
using Oxigin.Attendance.API.Abstractions;
using Oxigin.Attendance.Core.Interfaces.Managers;
using Oxigin.Attendance.Shared.Models.Entities;

namespace Oxigin.Attendance.API.Controllers;

/// <summary>
/// Provides endpoints for client request operations: create, approve, reject, and list requests.
/// </summary>
[ApiController]
[Route("[controller]")]
public class ClientRequestsController : BaseController
{
    private readonly IClientRequestManager _clientRequestManager;

    /// <summary>
    /// Constructor for ClientRequestsController.
    /// </summary>
    /// <param name="clientRequestManager">Injected client request manager.</param>
    /// <param name="logger">Logger instance.</param>
    public ClientRequestsController(IClientRequestManager clientRequestManager, ILogger<ClientRequestsController> logger)
        : base(logger)
    {
        _clientRequestManager = clientRequestManager;
    }

    /// <summary>
    /// Create a new client request.
    /// </summary>
    [HttpPost("create")]
    public async Task<IActionResult> CreateRequest([FromBody] ClientRequest request, CancellationToken token)
        => Ok(await _clientRequestManager.CreateRequestAsync(request, token));

    /// <summary>
    /// Get all requests for the current client.
    /// </summary>
    [HttpGet("mine")]
    public async Task<IActionResult> GetRequests(CancellationToken token)
        => Ok(await _clientRequestManager.GetRequestsForClientAsync(token));

    /// <summary>
    /// Approve a client request by ID.
    /// </summary>
    [HttpPost("approve/{id}")]
    public async Task<IActionResult> ApproveRequest(Guid id, CancellationToken token)
        => Ok(await _clientRequestManager.ApproveRequestAsync(id, token));

    /// <summary>
    /// Reject a client request by ID.
    /// </summary>
    [HttpPost("reject/{id}")]
    public async Task<IActionResult> RejectRequest(Guid id, CancellationToken token)
        => Ok(await _clientRequestManager.RejectRequestAsync(id, token));

    /// <summary>
    /// Get all pending approvals for the current user.
    /// </summary>
    [HttpGet("pending-approvals")]
    public async Task<IActionResult> GetPendingApprovals(CancellationToken token)
        => Ok(await _clientRequestManager.GetPendingApprovalsAsync(token));

    /// <summary>
    /// Approve a client request as a manager by ID.
    /// </summary>
    [HttpPost("manager/approve/{id}")]
    public async Task<IActionResult> ApproveRequestByManager(Guid id, CancellationToken token)
        => Ok(await _clientRequestManager.ApproveRequestByManagerAsync(id, token));

    /// <summary>
    /// Reject a client request as a manager by ID.
    /// </summary>
    [HttpPost("manager/reject/{id}")]
    public async Task<IActionResult> RejectRequestByManager(Guid id, CancellationToken token)
        => Ok(await _clientRequestManager.RejectRequestByManagerAsync(id, token));
}

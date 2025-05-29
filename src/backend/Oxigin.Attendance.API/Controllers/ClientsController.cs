using Microsoft.AspNetCore.Mvc;
using Oxigin.Attendance.API.Abstractions;
using Oxigin.Attendance.Core.Interfaces.Managers;
using Oxigin.Attendance.Shared.Models.Entities;
using Oxigin.Attendance.Shared.Models.Responses;

namespace Oxigin.Attendance.API.Controllers;

/// <summary>
/// Controller for job-related use cases (job requests, approvals, etc).
/// </summary>
[ApiController]
[Route("[controller]")]
public class JobController : BaseController
{
    /// <summary>
    /// The manager for job-related use cases.
    /// </summary>
    private readonly IClientsManager _clientsManager;

    /// <summary>
    /// Constructor for JobController.
    /// </summary>
    /// <param name="clientsManager">Manager for job use cases.</param>
    /// <param name="logger">Logger instance.</param>
    public JobController(IClientsManager clientsManager, ILogger<JobController> logger)
        : base(logger)
    {
        _clientsManager = clientsManager;
    }

    /// <summary>
    /// Retrieve all job requests for the current client or context.
    /// </summary>
    /// <param name="token">A token for cancelling downstream operations.</param>
    /// <returns>A collection of job requests.</returns>
    [HttpGet("jobrequests")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<JobRequest>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(StandardizedError))]
    public async Task<IActionResult> GetJobRequestsAsync(CancellationToken token)
    {
        var result = await _clientsManager.GetJobRequestsAsync(token);

        return Ok(result);
    }

    /// <summary>
    /// Create a new job request for a client, specifying event details and requirements.
    /// </summary>
    /// <param name="request">The job request entity containing event details, staff requirements, etc.</param>
    /// <param name="token">A token for cancelling downstream operations.</param>
    /// <returns>The created job request entity.</returns>
    [HttpPost("jobrequests")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JobRequest))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(StandardizedError))]
    public async Task<IActionResult> CreateJobRequestAsync([FromBody] JobRequest request, CancellationToken token)
    {
        var result = await _clientsManager.CreateJobRequestAsync(request, token);

        return Ok(result);
    }

    /// <summary>
    /// Approve a pending job request, typically by a client or site manager.
    /// </summary>
    /// <param name="request">The job request entity to approve (should include the id and any relevant context).</param>
    /// <param name="token">A token for cancelling downstream operations.</param>
    /// <returns>The updated job request entity with approved status.</returns>
    [HttpPost("jobrequests/approve")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JobRequest))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(StandardizedError))]
    public async Task<IActionResult> ApproveJobRequestAsync([FromBody] JobRequest request, CancellationToken token)
    {
        var result = await _clientsManager.ApproveJobRequestAsync(request, token);

        return Ok(result);
    }

    /// <summary>
    /// Reject a pending job request, typically by a client or site manager.
    /// </summary>
    /// <param name="request">The job request entity to reject (should include the id and any relevant context).</param>
    /// <param name="token">A token for cancelling downstream operations.</param>
    /// <returns>The updated job request entity with rejected status.</returns>
    [HttpPost("jobrequests/reject")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JobRequest))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(StandardizedError))]
    public async Task<IActionResult> RejectJobRequestAsync([FromBody] JobRequest request, CancellationToken token)
    {
        var result = await _clientsManager.RejectJobRequestAsync(request, token);

        return Ok(result);
    }
}

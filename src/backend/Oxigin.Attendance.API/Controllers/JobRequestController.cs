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
public class JobRequestController : BaseController
{
    /// <summary>
    /// The manager for job-related use cases.
    /// </summary>
    private readonly IJobRequestManager _jobRequestManager;

    /// <summary>
    /// Overloaded constructor to allow for injecting dependencies.
    /// </summary>
    /// <param name="jobRequestManager">Manager for job use cases.</param>
    /// <param name="logger">Logger instance.</param>
    public JobRequestController(IJobRequestManager jobRequestManager, ILogger<JobRequestController> logger)
        : base(logger)
    {
        _jobRequestManager = jobRequestManager;
    }

    /// <summary>
    /// Retrieve all job requests for the current client or context.
    /// </summary>
    /// <param name="token">A token for cancelling downstream operations.</param>
    /// <returns>A collection of job requests.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<JobRequest>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(StandardizedError))]
    public async Task<IActionResult> GetJobRequestsAsync(CancellationToken token)
    {
        var result = await _jobRequestManager.GetJobRequestsAsync(token);

        return Ok(result);
    }

    /// <summary>
    /// Create a new job request for a client, specifying event details and requirements.
    /// </summary>
    /// <param name="request">The job request entity containing event details, staff requirements, etc.</param>
    /// <param name="token">A token for cancelling downstream operations.</param>
    /// <returns>The created job request entity.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JobRequest))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(StandardizedError))]
    public async Task<IActionResult> CreateJobRequestAsync([FromBody] JobRequest request, CancellationToken token)
    {
        var result = await _jobRequestManager.CreateJobRequestAsync(request, token);

        return Ok(result);
    }

    /// <summary>
    /// Approve a pending job request, typically by a client or site manager.
    /// </summary>
    /// <param name="request">The job request entity to approve (should include the id and any relevant context).</param>
    /// <param name="token">A token for cancelling downstream operations.</param>
    /// <returns>The updated job request entity with approved status.</returns>
    [HttpPost("approve")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JobRequest))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(StandardizedError))]
    public async Task<IActionResult> ApproveJobRequestAsync([FromBody] JobRequest request, CancellationToken token)
    {
        var result = await _jobRequestManager.ApproveJobRequestAsync(request, token);

        return Ok(result);
    }

    /// <summary>
    /// Reject a pending job request, typically by a client or site manager.
    /// </summary>
    /// <param name="request">The job request entity to reject (should include the id and any relevant context).</param>
    /// <param name="token">A token for cancelling downstream operations.</param>
    /// <returns>The updated job request entity with rejected status.</returns>
    [HttpPost("reject")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JobRequest))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(StandardizedError))]
    public async Task<IActionResult> RejectJobRequestAsync([FromBody] JobRequest request, CancellationToken token)
    {
        var result = await _jobRequestManager.RejectJobRequestAsync(request, token);

        return Ok(result);
    }
}

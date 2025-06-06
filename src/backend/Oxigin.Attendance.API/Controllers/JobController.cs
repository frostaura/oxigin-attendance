using Microsoft.AspNetCore.Mvc;
using Oxigin.Attendance.API.Abstractions;
using Oxigin.Attendance.Core.Interfaces.Managers;
using Oxigin.Attendance.Datastore.Interfaces;
using Oxigin.Attendance.Shared.Models.Entities;
using Oxigin.Attendance.Shared.Models.Responses;
using Oxigin.Attendance.Shared.Enums;
using Oxigin.Attendance.Shared.Models.DTOs;

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
    private readonly IJobRequestManager _jobRequestManager;

    /// <summary>
    /// Overloaded constructor to allow for injecting dependencies.
    /// </summary> 
    /// <param name="jobRequestManager">Manager for job use cases.</param>
    /// <param name="logger">Logger instance.</param>
    public JobController(IJobRequestManager jobRequestManager, ILogger<JobController> logger, IDatastoreContext db)
        : base(logger, db)
    {
        _jobRequestManager = jobRequestManager;
    }

    /// <summary>
    /// Retrieve all job requests for the current client or context.
    /// </summary>
    /// <param name="token">A token for cancelling downstream operations.</param>
    /// <returns>A collection of job requests.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Job>))]
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Job))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(StandardizedError))]
    public async Task<IActionResult> CreateJobRequestAsync([FromBody] Job request, CancellationToken token)
    {
        var signedInUser = await GetRequestingUserAsync(token);
        if (signedInUser == null) 
            return Unauthorized("You must be logged in to create a job request.");

        // For non-admin users, they must be associated with a client
        if (signedInUser.UserType != UserType.Admin)
        {
            if (signedInUser.Client == null)
                return BadRequest(new StandardizedError 
                { 
                    Origin = nameof(JobController),
                    Message = "You must be associated with a client to create job requests.",
                    Data = new Dictionary<string, object>
                    {
                        { "UserId", signedInUser.Id },
                        { "UserType", signedInUser.UserType }
                    }
                });
            
            // Non-admin users can only create jobs for their associated client
            request.ClientID = signedInUser.Client.Id;
        }
        // For admin users, use whatever client ID is provided in the request

        request.RequestorID = signedInUser.Id;
        var result = await _jobRequestManager.CreateJobRequestAsync(request, token);

        return Ok(result);
    }

    /// <summary>
    /// Approve a pending job request, typically by a client or site manager.
    /// </summary>
    /// <param name="request">The job request entity to approve (should include the id and any relevant context).</param>
    /// <param name="token">A token for cancelling downstream operations.</param>
    /// <returns>The updated job request entity with approved status.</returns>
    [HttpPatch("approve")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Job))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(StandardizedError))]
    public async Task<IActionResult> ApproveJobRequestAsync([FromBody] JobStatusUpdate request, CancellationToken token)
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
    [HttpPatch("reject")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Job))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(StandardizedError))]
    public async Task<IActionResult> RejectJobRequestAsync([FromBody] JobStatusUpdate request, CancellationToken token)
    {
        var result = await _jobRequestManager.RejectJobRequestAsync(request, token);

        return Ok(result);
    }

    /// <summary>
    /// Get all jobs that require approval by the current user (e.g., site manager or client).
    /// </summary>
    /// <param name="token">A token for cancelling downstream operations.</param>
    /// <returns>A collection of jobs requiring approval.</returns>
    [HttpGet("requiring-approval")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Job>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(StandardizedError))]
    public async Task<IActionResult> GetJobsRequiringApprovalAsync(CancellationToken token)
    {
        var signedInUser = await GetRequestingUserAsync(token);
        if (signedInUser == null) return Unauthorized("You are not signed in.");
        var result = await _jobRequestManager.GetJobsRequiringApprovalAsync(signedInUser, token);
        return Ok(result);
    }

    /// <summary>
    /// Get all jobs that are awaiting confirmation by the current user (e.g., worker or client).
    /// </summary>
    /// <param name="token">A token for cancelling downstream operations.</param>
    /// <returns>A collection of jobs awaiting confirmation.</returns>
    [HttpGet("awaiting-confirmation")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Job>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(StandardizedError))]
    public async Task<IActionResult> GetJobsAwaitingConfirmationAsync(CancellationToken token)
    {
        var signedInUser = await GetRequestingUserAsync(token);
        if (signedInUser == null) return Unauthorized("You are not signed in.");
        var result = await _jobRequestManager.GetJobsAwaitingConfirmationAsync(signedInUser, token);
        return Ok(result);
    }
}

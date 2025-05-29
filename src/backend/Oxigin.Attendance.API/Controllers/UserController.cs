using FrostAura.Libraries.Core.Extensions.Validation;
using Microsoft.AspNetCore.Mvc;
using Oxigin.Attendance.API.Abstractions;
using Oxigin.Attendance.Core.Interfaces.Managers;
using Oxigin.Attendance.Shared.Models.Requests;
using Oxigin.Attendance.Shared.Models.Responses;

namespace Oxigin.Attendance.API.Controllers;

[ApiController]
[Route("[controller]")]
public class LotteryController : BaseController
{
    /// <summary>
    /// A manager to facilitate user-related use-cases.
    /// </summary>
    private readonly IUserManager _userManager;

    /// <summary>
    /// Overloaded constructor to allow for injecting dependencies.
    /// </summary>
    /// <param name="userManager">A manager to facilitate user-related use cases.</param>
    /// <param name="logger">The controller logger instance.</param>
    public LotteryController(IUserManager userManager, ILogger<LotteryController> logger)
        : base(logger)
    {
        _userManager = userManager.ThrowIfNull(nameof(userManager));
    }

    /// <summary>
    /// Sign in a user given a sign in request object.
    /// </summary>
    /// <param name="request">The user's sign in request.</param>
    /// <param name="token">A token for cancelling downstream operations.</param>
    /// <returns>A sign in response if successful, otherwise a 403.</returns>
    [HttpPost("SignIn")]
    public async Task<IActionResult> SignInAsync(UserSigninRequest request, CancellationToken token)
    {
        var response = await _userManager.SignInAsync(request, token);

        if (response.IsSuccess) return Unauthorized();

        return Ok(response);
    }
}
using Microsoft.AspNetCore.Mvc;
using Oxigin.Attendance.API.Abstractions;
using Oxigin.Attendance.Core.Interfaces.Managers;
using Oxigin.Attendance.Shared.Models.Requests;

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
        _userManager = userManager;
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

    /// <summary>
    /// Sign up a new user given a User entity.
    /// </summary>
    /// <param name="user">The user entity to register.</param>
    /// <param name="token">A token for cancelling downstream operations.</param>
    /// <returns>An IActionResult indicating success or failure.</returns>
    [HttpPost("SignUp")]
    public async Task<IActionResult> SignUpAsync([FromBody] Oxigin.Attendance.Shared.Models.Entities.User user, CancellationToken token)
    {
        var createdUser = await _userManager.SignUpAsync(user, token);
        if (createdUser == null) return BadRequest();
        return Ok(createdUser);
    }
}
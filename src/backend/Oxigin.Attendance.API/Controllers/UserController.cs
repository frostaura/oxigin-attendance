using Microsoft.AspNetCore.Mvc;
using Oxigin.Attendance.API.Abstractions;
using Oxigin.Attendance.Core.Interfaces.Managers;
using Oxigin.Attendance.Shared.Exceptions;
using Oxigin.Attendance.Shared.Models.Entities;
using Oxigin.Attendance.Shared.Models.Requests;
using Oxigin.Attendance.Shared.Models.Responses;

namespace Oxigin.Attendance.API.Controllers;

/// <summary>
/// Controller for user-related actions such as sign in, sign up, and password changes.
/// </summary>
[ApiController]
[Route("[controller]")]
public class UserController : BaseController
{
    /// <summary>
    /// The user manager service for handling user operations.
    /// </summary>
    private readonly IUserManager _userManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserController"/> class.
    /// </summary>
    /// <param name="userManager">The user manager service.</param>
    /// <param name="logger">The logger instance.</param>
    public UserController(IUserManager userManager, ILogger<UserController> logger)
        : base(logger)
    {
        _userManager = userManager;
    }

    /// <summary>
    /// Signs in a user with the provided credentials.
    /// </summary>
    /// <param name="request">The credentials for sign in.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>User sign-in response or error.</returns>
    [HttpPost("SignIn")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserSigninResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(StandardizedError))]
    public async Task<IActionResult> SignInAsync([FromBody] Credentials request, CancellationToken token)
    {
        try
        {
            var response = await _userManager.SignInAsync(request, token);

            return Ok(response);
        }
        catch (StandardizedErrorException e)
        {
            return BadRequest(e.Error);
        }
    }

    /// <summary>
    /// Registers a new user.
    /// </summary>
    /// <param name="user">The user to register.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>User sign-in response or error.</returns>
    [HttpPost("SignUp")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserSigninResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(StandardizedError))]
    public async Task<IActionResult> SignUpAsync([FromBody] User user, CancellationToken token)
    {
        try
        {
            var response = await _userManager.SignUpAsync(user, token);

            return Ok(response);
        }
        catch (StandardizedErrorException e)
        {
            return BadRequest(e.Error);
        }
    }

    /// <summary>
    /// Changes the password for a user.
    /// </summary>
    /// <param name="request">The credentials for password change.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>User sign-in response or error.</returns>
    [HttpPost("ChangePassword")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserSigninResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(StandardizedError))]
    public async Task<IActionResult> ChangePasswordAsync([FromBody] Credentials request, CancellationToken token)
    {
        try
        {
            var response = await _userManager.ChangePasswordAsync(request, token);
            return Ok(response);
        }
        catch (StandardizedErrorException e)
        {
            return BadRequest(e.Error);
        }
    }
}
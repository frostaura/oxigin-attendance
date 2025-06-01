using Microsoft.AspNetCore.Mvc;
using Oxigin.Attendance.API.Abstractions;
using Oxigin.Attendance.Core.Interfaces.Managers;
using Oxigin.Attendance.Shared.Exceptions;
using Oxigin.Attendance.Shared.Models.Entities;
using Oxigin.Attendance.Shared.Models.Requests;
using Oxigin.Attendance.Shared.Models.Responses;

namespace Oxigin.Attendance.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : BaseController
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
    public UserController(IUserManager userManager, ILogger<UserController> logger)
        : base(logger)
    {
        _userManager = userManager;
    }

    /// <summary>
    /// Sign in a user given a sign in request object.
    /// </summary>
    /// <param name="request">The user's sign in request.</param>
    /// <param name="token">A token for cancelling downstream operations.</param>
    /// <returns>A sign in response if successful, otherwise a bad request.</returns>
    [HttpPost("SignIn")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserSigninResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(StandardizedError))]
    public async Task<IActionResult> SignInAsync([FromBody] UserSigninRequest request, CancellationToken token)
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
    /// Sign up a new user given a User entity.
    /// </summary>
    /// <param name="user">The user entity to register.</param>
    /// <param name="token">A token for cancelling downstream operations.</param>
    /// <returns>An IActionResult indicating success or failure.</returns>
    [HttpPost("SignUp")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserSession))]
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
}
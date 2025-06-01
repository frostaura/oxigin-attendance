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
    private readonly IUserManager _userManager;

    public UserController(IUserManager userManager, ILogger<UserController> logger)
        : base(logger)
    {
        _userManager = userManager;
    }

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

    /// <returns>An IActionResult indicating success or failure.</returns>
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
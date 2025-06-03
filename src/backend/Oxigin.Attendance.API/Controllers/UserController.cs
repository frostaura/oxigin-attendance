using Microsoft.AspNetCore.Mvc;
using Oxigin.Attendance.API.Abstractions;
using Oxigin.Attendance.Core.Interfaces.Managers;
using Oxigin.Attendance.Datastore.Interfaces;
using Oxigin.Attendance.Shared.Exceptions;
using Oxigin.Attendance.Shared.Models.Entities;
using Oxigin.Attendance.Shared.Models.Requests;
using Oxigin.Attendance.Shared.Models.Responses;
using Microsoft.EntityFrameworkCore;

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
    public UserController(IUserManager userManager, ILogger<UserController> logger, IDatastoreContext db)
        : base(logger, db)
    {
        _userManager = userManager;
    }

    /// <summary>
    /// Get all users.
    /// </summary>
    /// <param name="token">Cancellation token.</param>
    /// <returns>List of User entities.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken token)
    {
        var signedInUser = await GetRequestingUserAsync(token);
        if (signedInUser == null) return Forbid();
        
        var users = await datastoreContext.Users
            .Include(u => u.Client)
            .Where(u => !u.Deleted)
            .ToListAsync(token);
            
        return Ok(users);
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

    /// <summary>
    /// Creates a new session given an existing one.
    /// </summary>
    /// <param name="token">Cancellation token.</param>
    /// <returns>User sign-in response or error.</returns>
    [HttpPost("RefreshSession")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserSigninResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(StandardizedError))]
    public async Task<IActionResult?> RefreshSession(CancellationToken token)
    {
        try
        {
            var signedInUser = await GetRequestingUserAsync(token);
            
            if (signedInUser == null) return Unauthorized("You are not signed in.");
            
            var response = await _userManager.RefreshSessionAsync(signedInUser, token);
            return Ok(response);
        }
        catch (StandardizedErrorException e)
        {
            return BadRequest(e.Error);
        }
    }

    /// <summary>
    /// Updates an existing user's details.
    /// </summary>
    /// <param name="user">The user entity with updated details.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The updated user entity or error.</returns>
    [HttpPatch]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(StandardizedError))]
    public async Task<IActionResult> UpdateUserAsync([FromBody] User user, CancellationToken token)
    {
        try
        {
            var updatedUser = await _userManager.UpdateUserAsync(user, token);
            return Ok(updatedUser);
        }
        catch (StandardizedErrorException e)
        {
            return BadRequest(e.Error);
        }
    }

    /// <summary>
    /// Delete a user by ID.
    /// </summary>
    /// <param name="id">The User ID.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>No content if successful.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteUser(Guid id, CancellationToken token)
    {
        var signedInUser = await GetRequestingUserAsync(token);
        if (signedInUser == null) return Forbid();

        var user = await datastoreContext.Users.FirstOrDefaultAsync(u => u.Id == id, token);
        if (user == null || user.Deleted) return NotFound();

        // Soft delete the user
        user.Deleted = true;
        await datastoreContext.SaveChangesAsync(token);

        return NoContent();
    }
}
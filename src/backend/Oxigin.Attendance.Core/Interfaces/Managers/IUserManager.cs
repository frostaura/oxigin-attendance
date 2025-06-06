using Oxigin.Attendance.Shared.Models.Entities;
using Oxigin.Attendance.Shared.Models.Requests;
using Oxigin.Attendance.Shared.Models.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace Oxigin.Attendance.Core.Interfaces.Managers;

/// <summary>
/// A manager for handling user-related use cases.
/// </summary>
public interface IUserManager
{
    /// <summary>
    /// Sign in a user given a sign in request object.
    /// </summary>
    /// <param name="request">The user's sign in request.</param>
    /// <param name="token">A token for cancelling downstream operations.</param>
    /// <returns>A sign in response.</returns>
    public Task<UserSigninResponse> SignInAsync(Credentials request, CancellationToken token);

    /// <summary>
    /// Sign up a user given a user entity and get a fresh session for them.
    /// </summary>
    /// <param name="user">The user entity containing user details.</param>
    /// <param name="token">A token for cancelling downstream operations.</param>
    /// <returns>The created user's new session.</returns>
    Task<UserSigninResponse> SignUpAsync(User user, CancellationToken token);
    Task<UserSigninResponse> ChangePasswordAsync(Credentials request, CancellationToken token);
    /// <summary>
    /// Refreshes the session for a signed-in user, creating a new session and returning updated context.
    /// </summary>
    /// <param name="user">The signed-in user entity.</param>
    /// <param name="token">A token for cancelling downstream operations.</param>
    /// <returns>The user's new session context.</returns>
    Task<UserSigninResponse> RefreshSessionAsync(User user, CancellationToken token);

    /// <summary>
    /// Updates an existing user's details.
    /// </summary>
    /// <param name="user">The user entity with updated details.</param>
    /// <param name="token">A token for cancelling downstream operations.</param>
    /// <returns>The updated user entity.</returns>
    Task<User> UpdateUserAsync(User user, CancellationToken token);
}
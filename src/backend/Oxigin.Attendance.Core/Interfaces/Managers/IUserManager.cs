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
    public Task<UserSigninResponse> SignInAsync(UserSigninRequest request, CancellationToken token);

    /// <summary>
    /// Sign up a user given a user entity and get a fresh session for them.
    /// </summary>
    /// <param name="user">The user entity containing user details.</param>
    /// <param name="token">A token for cancelling downstream operations.</param>
    /// <returns>The created user's new session.</returns>
    Task<UserSession> SignUpAsync(User user, CancellationToken token);
}
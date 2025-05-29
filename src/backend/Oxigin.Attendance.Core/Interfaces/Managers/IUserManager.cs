using Oxigin.Attendance.Shared.Models.Requests;
using Oxigin.Attendance.Shared.Models.Responses;

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
}
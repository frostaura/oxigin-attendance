using Oxigin.Attendance.Shared.Models.Entities;

namespace Oxigin.Attendance.Shared.Models.Responses;

/// <summary>
/// The response object from a successful sign in request.
/// </summary>
public class UserSigninResponse
{
    /// <summary>
    /// Whether the sign in request succeeded.
    /// </summary>
    public bool IsSuccess { get; set; }
    /// <summary>
    /// The unique session id for a signed in user.
    /// </summary>
    public string SessionId { get; set; }
    /// <summary>
    /// The signed in user's context.
    /// </summary>
    public User User { get; set; }
}
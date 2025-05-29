using System.Diagnostics;

namespace Oxigin.Attendance.Shared.Models.Requests;

[DebuggerDisplay("User: {Email}")]
public class UserSigninRequest
{
    /// <summary>
    /// The user's email address to sign in with.
    /// </summary>
    public string Email { get; set; }
    /// <summary>
    /// The user's password.
    /// </summary>
    public string Password { get; set; }
}
namespace Oxigin.Attendance.Shared.Models.Responses;

/// <summary>
/// A standardized error object for consistency.
/// </summary>
public class StandardizedError
{
    /// <summary>
    /// The origin object responsible for producing the error.
    /// </summary>
    public string Origin { get; set; }
    /// <summary>
    /// The error message text.
    /// </summary>
    public string Message { get; set; }
    /// <summary>
    /// Optional additional data to be included with the error as context.
    /// </summary>
    public Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();
}
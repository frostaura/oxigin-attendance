using Oxigin.Attendance.Shared.Models.Responses;

namespace Oxigin.Attendance.Shared.Exceptions;

/// <summary>
/// A custom exception that allows for transporting a standardized error.
/// </summary>
public class StandardizedErrorException : Exception
{
    public StandardizedError Error { get; set; }
}
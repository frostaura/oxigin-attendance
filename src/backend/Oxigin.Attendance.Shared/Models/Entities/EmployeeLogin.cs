// File: EmployeeLogin.cs
// Represents login data for an employee.
using System.Diagnostics;

namespace Oxigin.Attendance.Shared.Models.Entities
{
    /// <summary>
    /// Employee login model.
    /// </summary>
    [DebuggerDisplay("{Email}")]
    public class EmployeeLogin
    {
        /// <summary>Email address.</summary>
        public string Email { get; set; }
        /// <summary>Password.</summary>
        public string Password { get; set; }
    }
}

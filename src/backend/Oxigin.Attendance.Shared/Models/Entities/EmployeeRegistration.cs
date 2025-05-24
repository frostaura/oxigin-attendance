// File: EmployeeRegistration.cs
// Represents registration data for a new employee.
using System.Diagnostics;

namespace Oxigin.Attendance.Shared.Models.Entities
{
    /// <summary>
    /// Employee registration model.
    /// </summary>
    [DebuggerDisplay("{FirstName} {LastName}")]
    public class EmployeeRegistration
    {
        /// <summary>First name.</summary>
        public string FirstName { get; set; }
        /// <summary>Last name.</summary>
        public string LastName { get; set; }
        /// <summary>ID number.</summary>
        public string IdNumber { get; set; }
        /// <summary>Contact number.</summary>
        public string Contact { get; set; }
        /// <summary>Email address.</summary>
        public string Email { get; set; }
        /// <summary>Password.</summary>
        public string Password { get; set; }
        /// <summary>Role or worker type.</summary>
        public string Role { get; set; }
    }
}

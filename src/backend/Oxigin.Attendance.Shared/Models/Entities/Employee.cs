// File: Employee.cs
// Represents an employee in the Oxigin Attendance system.
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Oxigin.Attendance.Shared.Models.Entities
{
    /// <summary>
    /// Employee entity model.
    /// </summary>
    [DebuggerDisplay("Name: {FirstName} {LastName}")]
    public class Employee
    {
        /// <summary>Employee unique identifier.</summary>
        public string Id { get; set; }
        /// <summary>First name of the employee.</summary>
        public string FirstName { get; set; }
        /// <summary>Last name of the employee.</summary>
        public string LastName { get; set; }
        /// <summary>ID number (national or company ID).</summary>
        public string IdNumber { get; set; }
        /// <summary>Contact number.</summary>
        public string Contact { get; set; }
        /// <summary>Address.</summary>
        public string Address { get; set; }
        /// <summary>Email address.</summary>
        public string Email { get; set; }
        /// <summary>Role or worker type.</summary>
        public string Role { get; set; }
    }
}

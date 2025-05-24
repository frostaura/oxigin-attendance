// File: Client.cs
// Represents a client in the Oxigin Attendance system.
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Oxigin.Attendance.Shared.Models.Entities
{
    /// <summary>
    /// Client entity model.
    /// </summary>
    [DebuggerDisplay("Name: {Name}")]
    public class Client
    {
        /// <summary>Client unique identifier.</summary>
        public string Id { get; set; }
        /// <summary>Name of the client or company.</summary>
        public string Name { get; set; }
        /// <summary>Registration number.</summary>
        public string RegistrationNo { get; set; }
        /// <summary>Contact number.</summary>
        public string Contact { get; set; }
        /// <summary>Email address.</summary>
        public string Email { get; set; }
        /// <summary>Address.</summary>
        public string Address { get; set; }
    }
}

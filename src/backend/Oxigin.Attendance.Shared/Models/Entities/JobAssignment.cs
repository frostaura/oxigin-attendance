// File: JobAssignment.cs
// Represents an assignment of an employee to a job.
using System;
using System.Diagnostics;

namespace Oxigin.Attendance.Shared.Models.Entities
{
    /// <summary>
    /// Job assignment entity model.
    /// </summary>
    [DebuggerDisplay("Job: {JobId} Employee: {EmployeeId}")]
    public class JobAssignment
    {
        /// <summary>Assignment unique identifier.</summary>
        public string Id { get; set; }
        /// <summary>Job identifier.</summary>
        public string JobId { get; set; }
        /// <summary>Employee identifier.</summary>
        public string EmployeeId { get; set; }
        /// <summary>Role or worker type.</summary>
        public string Role { get; set; }
        /// <summary>Date assigned.</summary>
        public DateTime AssignedDate { get; set; }
    }
}

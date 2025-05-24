// File: CheckInRecord.cs
// Represents a record of an employee's check-in and check-out for a job.
using System;
using System.Diagnostics;

namespace Oxigin.Attendance.Shared.Models.Entities
{
    /// <summary>
    /// Check-in record model.
    /// </summary>
    [DebuggerDisplay("Job: {JobId} Employee: {EmployeeId}")]
    public class CheckInRecord
    {
        /// <summary>Record unique identifier.</summary>
        public string Id { get; set; }
        /// <summary>Job identifier.</summary>
        public string JobId { get; set; }
        /// <summary>Employee identifier.</summary>
        public string EmployeeId { get; set; }
        /// <summary>Time in.</summary>
        public DateTime TimeIn { get; set; }
        /// <summary>Time out.</summary>
        public DateTime? TimeOut { get; set; }
        /// <summary>GPS coordinates at check-in.</summary>
        public string GpsCoordinates { get; set; }
        /// <summary>Facial recognition image URL or data (optional).</summary>
        public string FaceImage { get; set; }
    }
}

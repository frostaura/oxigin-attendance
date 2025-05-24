// File: CheckInData.cs
// Represents check-in data submitted by an employee.
using System;
using System.Diagnostics;

namespace Oxigin.Attendance.Shared.Models.Entities
{
    /// <summary>
    /// Check-in data model.
    /// </summary>
    [DebuggerDisplay("Job: {JobId} Employee: {EmployeeId}")]
    public class CheckInData
    {
        /// <summary>Job identifier.</summary>
        public string JobId { get; set; }
        /// <summary>Employee identifier.</summary>
        public string EmployeeId { get; set; }
        /// <summary>GPS coordinates at check-in.</summary>
        public string GpsCoordinates { get; set; }
        /// <summary>Timestamp of check-in.</summary>
        public DateTime TimeIn { get; set; }
        /// <summary>Facial recognition image URL or data (optional).</summary>
        public string FaceImage { get; set; }
    }
}

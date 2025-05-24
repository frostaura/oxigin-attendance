// File: TimesheetRequest.cs
// Represents a request for a timesheet (by date, job, or employee).
using System;
using System.Diagnostics;

namespace Oxigin.Attendance.Shared.Models.Entities
{
    /// <summary>
    /// Timesheet request model.
    /// </summary>
    [DebuggerDisplay("Employee: {EmployeeId} Date: {Date}")]
    public class TimesheetRequest
    {
        /// <summary>Employee identifier (optional).</summary>
        public string EmployeeId { get; set; }
        /// <summary>Job identifier (optional).</summary>
        public string JobId { get; set; }
        /// <summary>Date for the timesheet (optional).</summary>
        public DateTime? Date { get; set; }
        /// <summary>Timeframe (Daily, Weekly, etc.).</summary>
        public string Timeframe { get; set; }
    }
}

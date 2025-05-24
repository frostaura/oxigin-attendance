// File: StaffAllocationRequest.cs
// Represents a request to allocate staff to a job.
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Oxigin.Attendance.Shared.Models.Requests
{
    /// <summary>
    /// Request model for allocating staff to a job.
    /// </summary>
    [DebuggerDisplay("Job: {JobId} Employees: {EmployeeIds?.Count}")]
    public class StaffAllocationRequest
    {
        /// <summary>Job identifier.</summary>
        public string JobId { get; set; }
        /// <summary>List of employee IDs to allocate.</summary>
        public List<string> EmployeeIds { get; set; }
    }
}

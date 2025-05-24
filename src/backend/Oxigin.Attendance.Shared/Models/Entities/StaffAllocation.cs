// File: StaffAllocation.cs
// Represents the allocation of staff to a job/event.
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Oxigin.Attendance.Shared.Models.Entities
{
    /// <summary>
    /// Staff allocation model.
    /// </summary>
    [DebuggerDisplay("Job: {JobId} Employees: {EmployeeIds?.Count}")]
    public class StaffAllocation
    {
        /// <summary>Allocation unique identifier.</summary>
        public string Id { get; set; }
        /// <summary>Job identifier.</summary>
        public string JobId { get; set; }
        /// <summary>List of allocated employee IDs.</summary>
        public List<string> EmployeeIds { get; set; }
        /// <summary>Date allocated.</summary>
        public DateTime AllocatedDate { get; set; }
    }
}

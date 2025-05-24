// File: DashboardStats.cs
// Represents summary statistics for the admin dashboard.
using System.Diagnostics;

namespace Oxigin.Attendance.Shared.Models.Entities
{
    /// <summary>
    /// Dashboard statistics model.
    /// </summary>
    [DebuggerDisplay("Jobs: {TotalJobs} Employees: {TotalEmployees}")]
    public class DashboardStats
    {
        /// <summary>Total number of jobs.</summary>
        public int TotalJobs { get; set; }
        /// <summary>Total number of employees.</summary>
        public int TotalEmployees { get; set; }
        /// <summary>Total number of clients.</summary>
        public int TotalClients { get; set; }
        /// <summary>Total check-ins today.</summary>
        public int CheckInsToday { get; set; }
        /// <summary>Total pending requests.</summary>
        public int PendingRequests { get; set; }
    }
}

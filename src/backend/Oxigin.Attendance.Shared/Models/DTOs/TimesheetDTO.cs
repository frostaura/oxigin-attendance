using System;

namespace Oxigin.Attendance.Shared.Models.DTOs;

/// <summary>
/// DTO for creating a new timesheet entry
/// </summary>
public class CreateTimesheetDTO
{
    /// <summary>
    /// Date and time of check-in
    /// </summary>
    public DateTime TimeIn { get; set; }

    /// <summary>
    /// ID of the job being worked on
    /// </summary>
    public Guid JobID { get; set; }

    /// <summary>
    /// ID of the employee checking in
    /// </summary>
    public Guid EmployeeID { get; set; }

    /// <summary>
    /// ID of the site manager for this job
    /// </summary>
    public Guid SiteManagerID { get; set; }
} 
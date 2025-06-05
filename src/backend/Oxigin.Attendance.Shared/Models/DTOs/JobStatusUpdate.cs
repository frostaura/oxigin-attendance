using System;

namespace Oxigin.Attendance.Shared.Models.DTOs;

/// <summary>
/// DTO for creating a new timesheet entry
/// </summary>
public class JobStatusUpdate
{
    /// <summary>
    /// ID of the employee checking in
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// ID of the site manager for this job
    /// </summary>
    public bool Approved { get; set; }
} 
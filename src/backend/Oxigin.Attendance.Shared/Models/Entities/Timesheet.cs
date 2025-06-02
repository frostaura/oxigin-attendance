using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Oxigin.Attendance.Shared.Enums;
using Oxigin.Attendance.Shared.Models.Abstractions;

namespace Oxigin.Attendance.Shared.Models.Entities;

/// <summary>
/// An entity representing timesheet entries
/// </summary>
[Table("Timesheets")]
public class Timesheet : BaseEntity
{
    /// <summary>
    /// Date and time of check-in.
    /// </summary>
    [Required]
    public DateTime TimeIn { get; set; }
    /// <summary>
    /// Date and time of check-out.
    /// </summary>
    public DateTime TimeOut { get; set; }
    
    /// <summary>
    /// Foreign key to the Jobs table.
    ///  Renamed to JobID.
    /// </summary>
    public Guid JobID { get; set; }

    /// <summary>
    /// Navigation property for the related Job.
    /// </summary>
    [ForeignKey(nameof(JobID))]
    public virtual Job Job { get; set; }
    
    /// <summary>
    /// Foreign key to the Employee table.
    ///  Renamed to EmployeeID.
    /// </summary>
    public Guid EmployeeID { get; set; }

    /// <summary>
    /// Navigation property for the related Employee.
    /// </summary>
    [ForeignKey(nameof(EmployeeID))]
    public virtual Employee Employee { get; set; }
    
    /// <summary>
    /// Foreign key to the SiteManager ID.
    ///  Renamed to SiteManagerID.
    /// </summary>
    public Guid SiteManagerID { get; set; }

    /// <summary>
    /// Navigation property for the related SiteManager.
    /// </summary>
    [ForeignKey(nameof(SiteManagerID))]
    public virtual User User { get; set; }

}
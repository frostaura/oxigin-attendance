using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Oxigin.Attendance.Shared.Models.Abstractions;

namespace Oxigin.Attendance.Shared.Models.Entities;

/// <summary>
/// An entity representing job allocations in the database.
/// </summary>
[Table("Allocations")]
public class Allocation : BaseNamedEntity
{
    /// <summary>
    /// A brief description of the allocation taken from worker type.
    /// </summary>
    public string? Description { get; set; }
    /// <summary>
    /// The date and time of the allocation.
    /// </summary>
    [Required]
    public DateTime Time { get; set; }
    /// <summary>
    /// The number of hours needed for this allocation.
    /// </summary>
    [Required]
    public int HoursNeeded { get; set; }
    /// <summary>
    /// Foreign key to the Jobs table.
    /// Renamed to JobID.
    /// </summary>
    public Guid JobID { get; set; }
    /// <summary>
    /// Navigation property for the related Job.
    /// </summary>
    [ForeignKey(nameof(JobID))]
    public virtual Job Job { get; set; }
    /// <summary>
    /// Foreign key to the Employees table.
    /// Renamed to EmployeeID.
    /// </summary>
    public Guid EmployeeID { get; set; }
    /// <summary>
    /// Navigation property for the related Employee.
    /// </summary>
    [ForeignKey(nameof(EmployeeID))]
    public virtual Employee Employee { get; set; }
}
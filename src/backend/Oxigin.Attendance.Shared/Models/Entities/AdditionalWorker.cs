using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oxigin.Attendance.Shared.Models.Abstractions;

namespace Oxigin.Attendance.Shared.Models.Entities;

/// <summary>
/// An entity representing additional workers
/// </summary>
[Table("AdditionalWorkers")]
public class AdditionalWorker : BaseEntity
{
    /// <summary>
    /// The unique contact number of the user.
    /// </summary>
    [Required(AllowEmptyStrings = false, ErrorMessage = $"A worker type is required.")]
    public string WorkerType { get; set; }

    /// <summary>
    /// The unique email for the user.
    /// </summary>
    [Required(AllowEmptyStrings = false, ErrorMessage = $"Number of workers needed.")]
    public int NumWorkers { get; set; }
    
    
    /// <summary>
    /// The unique email for the user.
    /// </summary>
    [Required(AllowEmptyStrings = false, ErrorMessage = $"Number of hours needed.")]
    public int NumHours { get; set; }
    
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

}

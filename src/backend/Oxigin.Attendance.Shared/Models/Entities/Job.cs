using System.ComponentModel.DataAnnotations.Schema;
using Oxigin.Attendance.Shared.Models.Abstractions;

namespace Oxigin.Attendance.Shared.Models.Entities;

/// <summary>
/// Represents a job request or assignment in the system.
/// </summary>
[Table("Jobs")]
public class Job : BaseEntity
{
    /// <summary>
    /// Foreign key to the user who requested the job.
    /// </summary>
    public Guid RequestorID { get; set; }
    /// <summary>
    /// Foreign key to the client for whom the job is requested.
    /// </summary>
    public Guid ClientID { get; set; }
    /// <summary>
    /// Optional purchase order number associated with the job.
    /// </summary>
    public string PurchaseOrderNumber { get; set; }
    /// <summary>
    /// Name or title of the job.
    /// </summary>
    public string JobName { get; set; }
    /// <summary>
    /// Date and time when the job is scheduled.
    /// </summary>
    public DateTime Time { get; set; }
    /// <summary>
    /// Location where the job will take place (optional).
    /// </summary>
    public string? Location { get; set; }
    /// <summary>
    /// Number of workers required for the job.
    /// </summary>
    public int NumWorkers { get; set; }
    /// <summary>
    /// Number of hours estimated or allocated for the job.
    /// </summary>
    public int NumHours { get; set; }
    /// <summary>
    /// Indicates whether the job has been approved.
    /// </summary>
    public bool Approved { get; set; }
    /// <summary>
    /// The context for the user that is mapped to this session (requestor).
    /// </summary>
    [ForeignKey(nameof(RequestorID))]
    public virtual User Requestor { get; set; }
    /// <summary>
    /// The context for the user that is mapped to this session (client).
    /// </summary>
    [ForeignKey(nameof(ClientID))]
    public virtual User Client { get; set; }
}

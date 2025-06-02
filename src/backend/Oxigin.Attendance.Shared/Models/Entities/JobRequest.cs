using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oxigin.Attendance.Shared.Models.Abstractions;

namespace Oxigin.Attendance.Shared.Models.Entities;

/// <summary>
/// An entity representing a job request made by a client.
/// </summary>
[Table("JobRequests")]
public class JobRequest : BaseEntity
{
    /// <summary>
    /// The name or title of the job.
    /// </summary>
    [Required]
    public string JobName { get; set; }

    /// <summary>
    /// Name of the person requesting the job.
    /// </summary>
    [Required]
    public string RequestorName { get; set; }

    /// <summary>
    /// Purchase order number for the job request.
    /// </summary>
    [Required]
    public string PurchaseOrderNumber { get; set; }

    /// <summary>
    /// The date of the job.
    /// </summary>
    [Required]
    public DateTime Date { get; set; }

    /// <summary>
    /// The time of the job.
    /// </summary>
    [Required]
    public TimeSpan Time { get; set; }

    /// <summary>
    /// Location where the job will take place.
    /// </summary>
    [Required]
    public string Location { get; set; }

    /// <summary>
    /// Number of workers needed for the job.
    /// </summary>
    [Required]
    public int NumberOfWorkers { get; set; }

    /// <summary>
    /// Number of hours needed for the job.
    /// </summary>
    [Required]
    public int NumberOfHours { get; set; }

    /// <summary>
    /// Indicates whether the job request has been approved.
    /// </summary>
    public bool Approved { get; set; } = false;

    /// <summary>
    /// The unique identifier of the client who made the request.
    /// </summary>
    public Guid ClientId { get; set; }

    /// <summary>
    /// The client who made the job request.
    /// </summary>
    [ForeignKey(nameof(ClientId))]
    public virtual User Client { get; set; }
}

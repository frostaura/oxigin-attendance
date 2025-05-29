using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oxigin.Attendance.Shared.Enums;
using Oxigin.Attendance.Shared.Models.Abstractions;

namespace Oxigin.Attendance.Shared.Models.Entities;

/// <summary>
/// An entity representing a job request made by a client.
/// </summary>
[Table("JobRequests")]
public class JobRequest : BaseEntity
{
    /// <summary>
    /// The name or title of the job/event.
    /// </summary>
    [Required]
    public string Title { get; set; }
    /// <summary>
    /// The description or details of the job/event.
    /// </summary>
    public string Description { get; set; }
    /// <summary>
    /// The date and time the job/event is scheduled for.
    /// </summary>
    public DateTime ScheduledAt { get; set; }
    /// <summary>
    /// The number of staff required for the job/event.
    /// </summary>
    public int StaffRequired { get; set; }
    /// <summary>
    /// The status of the job request (e.g., Pending, Approved, Rejected).
    /// </summary>
    public JobRequestStatus Status { get; set; }
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

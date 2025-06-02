using System.ComponentModel.DataAnnotations.Schema;
using Oxigin.Attendance.Shared.Models.Abstractions;

namespace Oxigin.Attendance.Shared.Models.Entities;
[Table("Jobs")]
// Represents a job request or assignment in the system.
public class Job: BaseEntity
{ 
    // Foreign key to the user who requested the job
    public Guid RequestorID { get; set; } // FK

    // Foreign key to the client for whom the job is requested
    public Guid ClientID { get; set; } // FK

    // Optional purchase order number associated with the job
    public string PurchaseOrderNumber { get; set; }

    // Name or title of the job
    public string JobName { get; set; }

    // Date and time when the job is scheduled
    public DateTime Time { get; set; }

    // Location where the job will take place (optional)
    public string? Location { get; set; }

    // Number of workers required for the job
    public int NumWorkers { get; set; }

    // Number of hours estimated or allocated for the job
    public int NumHours { get; set; }

    // Indicates whether the job has been approved
    public bool Approved { get; set; }
    
    /// <summary>
    /// The context for the user that is mapped to this session.
    /// </summary>
    [ForeignKey(nameof(RequestorID))]
    public virtual User Requestor { get; set; }
    
    /// <summary>
    /// The context for the user that is mapped to this session.
    /// </summary>
    [ForeignKey(nameof(ClientID))]
    public virtual User Client { get; set; }
}

// File: ClientRequest.cs
// Represents a job request made by a client.
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Oxigin.Attendance.Shared.Models.Entities
{
    /// <summary>
    /// Client job request entity model.
    /// </summary>
    [DebuggerDisplay("Job: {JobName} for {ClientId}")]
    public class ClientRequest
    {
        /// <summary>Request unique identifier.</summary>
        public string Id { get; set; }
        /// <summary>Client identifier.</summary>
        public string ClientId { get; set; }
        /// <summary>Job name.</summary>
        public string JobName { get; set; }
        /// <summary>Purchase order number.</summary>
        public string PurchaseOrder { get; set; }
        /// <summary>Date of the event/job.</summary>
        public DateTime Date { get; set; }
        /// <summary>Time of the event/job.</summary>
        public string Time { get; set; }
        /// <summary>Location of the job.</summary>
        public string Location { get; set; }
        /// <summary>Number of workers needed.</summary>
        public int WorkersNeeded { get; set; }
        /// <summary>Hours needed.</summary>
        public int HoursNeeded { get; set; }
        /// <summary>Status (Pending, Approved, Rejected, etc.).</summary>
        public string Status { get; set; }
    }
}

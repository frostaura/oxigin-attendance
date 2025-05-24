// File: ClientRequestCreateRequest.cs
// Represents a request to create a new client job request.
using System;
using System.Diagnostics;

namespace Oxigin.Attendance.Shared.Models.Requests
{
    /// <summary>
    /// Request model for creating a client job request.
    /// </summary>
    [DebuggerDisplay("Job: {JobName} for {ClientId}")]
    public class ClientRequestCreateRequest
    {
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
    }
}

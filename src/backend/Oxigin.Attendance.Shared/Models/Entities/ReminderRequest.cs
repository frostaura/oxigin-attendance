// File: ReminderRequest.cs
// Represents a request to send a reminder notification.
using System;
using System.Diagnostics;

namespace Oxigin.Attendance.Shared.Models.Entities
{
    /// <summary>
    /// Reminder request model.
    /// </summary>
    [DebuggerDisplay("To: {Recipient} For: {JobId}")]
    public class ReminderRequest
    {
        /// <summary>Recipient identifier.</summary>
        public string Recipient { get; set; }
        /// <summary>Job identifier (optional).</summary>
        public string JobId { get; set; }
        /// <summary>Message content.</summary>
        public string Message { get; set; }
        /// <summary>Timestamp to send (optional).</summary>
        public DateTime? SendAt { get; set; }
    }
}

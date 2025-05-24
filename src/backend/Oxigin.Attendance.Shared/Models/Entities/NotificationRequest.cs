// File: NotificationRequest.cs
// Represents a request to send a notification.
using System;
using System.Diagnostics;

namespace Oxigin.Attendance.Shared.Models.Entities
{
    /// <summary>
    /// Notification request model.
    /// </summary>
    [DebuggerDisplay("To: {Recipient} Type: {Type}")]
    public class NotificationRequest
    {
        /// <summary>Recipient identifier (employee, client, etc.).</summary>
        public string Recipient { get; set; }
        /// <summary>Notification type (SMS, Email, etc.).</summary>
        public string Type { get; set; }
        /// <summary>Message content.</summary>
        public string Message { get; set; }
        /// <summary>Related job or event ID (optional).</summary>
        public string RelatedId { get; set; }
        /// <summary>Timestamp to send (optional).</summary>
        public DateTime? SendAt { get; set; }
    }
}

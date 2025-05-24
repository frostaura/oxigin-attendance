// File: NotificationRecord.cs
// Represents a record of a sent notification.
using System;
using System.Diagnostics;

namespace Oxigin.Attendance.Shared.Models.Entities
{
    /// <summary>
    /// Notification record model.
    /// </summary>
    [DebuggerDisplay("To: {Recipient} Type: {Type} Sent: {SentAt}")]
    public class NotificationRecord
    {
        /// <summary>Record unique identifier.</summary>
        public string Id { get; set; }
        /// <summary>Recipient identifier.</summary>
        public string Recipient { get; set; }
        /// <summary>Notification type (SMS, Email, etc.).</summary>
        public string Type { get; set; }
        /// <summary>Message content.</summary>
        public string Message { get; set; }
        /// <summary>Related job or event ID (optional).</summary>
        public string RelatedId { get; set; }
        /// <summary>Timestamp sent.</summary>
        public DateTime SentAt { get; set; }
        /// <summary>Status (Sent, Failed, etc.).</summary>
        public string Status { get; set; }
    }
}

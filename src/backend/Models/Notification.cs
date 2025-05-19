using System;

namespace OxiginAttendance.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string Recipient { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }
        public NotificationType Type { get; set; }
        public string NotificationType { get; set; }
    }

    public enum NotificationType
    {
        SMS,
        Email
    }
}

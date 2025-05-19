using System;

namespace Backend.Models
{
    public class ClientRequest
    {
        public int Id { get; set; }
        public DateTime EventDate { get; set; }
        public TimeSpan EventTime { get; set; }
        public int NumberOfStaff { get; set; }
        public string Role { get; set; }
        public string Location { get; set; }
        public string ApprovalStatus { get; set; }
        public string ClientInformation { get; set; }
        public bool IsClientApproved { get; set; }
        public bool IsSiteManagerApproved { get; set; }
    }
}

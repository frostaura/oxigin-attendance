using System;

namespace Backend.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string NotificationPreference { get; set; }
        public DateTime CheckInTime { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool ReceiveSmsNotifications { get; set; }
        public bool ReceiveEmailNotifications { get; set; }
    }
}

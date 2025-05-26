using Oxigin.Attendance.Shared.Enums;

namespace Oxigin.Attendance.Shared.Models.Entities
{
    public class UserRegistration
    {
        public UserType UserType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        // Add additional fields as needed for all user types
        // Optionally, use nullable fields or a Dictionary<string, object> for extensibility
    }
}

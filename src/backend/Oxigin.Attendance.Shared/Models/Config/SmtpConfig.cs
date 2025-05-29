namespace Oxigin.Attendance.Shared.Models.Config
{
    /// <summary>
    /// Configuration for SMTP email sending.
    /// </summary>
    public class SmtpConfig
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Pass { get; set; }
        public string From { get; set; }
    }
}

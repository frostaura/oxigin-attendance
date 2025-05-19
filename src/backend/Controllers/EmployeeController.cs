using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] EmployeeRegistrationDto registrationDto)
        {
            var result = await _employeeService.RegisterAsync(registrationDto);
            if (result)
            {
                return Ok(new { message = "Registration successful" });
            }
            return BadRequest(new { message = "Registration failed" });
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] EmployeeAuthenticationDto authenticationDto)
        {
            var token = await _employeeService.AuthenticateAsync(authenticationDto);
            if (token != null)
            {
                return Ok(new { token });
            }
            return Unauthorized(new { message = "Authentication failed" });
        }

        [HttpPost("notify")]
        public async Task<IActionResult> Notify([FromBody] NotificationDto notificationDto)
        {
            var result = await _employeeService.SendNotificationAsync(notificationDto);
            if (result)
            {
                return Ok(new { message = "Notification sent successfully" });
            }
            return BadRequest(new { message = "Failed to send notification" });
        }

        [HttpPost("checkin")]
        public async Task<IActionResult> CheckIn([FromBody] CheckInDto checkInDto)
        {
            var result = await _employeeService.CheckInAsync(checkInDto);
            if (result)
            {
                return Ok(new { message = "Check-in successful" });
            }
            return BadRequest(new { message = "Check-in failed" });
        }

        [HttpGet("notifications")]
        public async Task<IActionResult> GetNotifications()
        {
            var notifications = await _employeeService.GetNotificationsAsync();
            return Ok(notifications);
        }

        [HttpPost("register-employee")]
        public async Task<IActionResult> RegisterEmployee([FromBody] EmployeeRegistrationDto registrationDto)
        {
            var result = await _employeeService.RegisterEmployeeAsync(registrationDto);
            if (result)
            {
                return Ok(new { message = "Employee registration successful" });
            }
            return BadRequest(new { message = "Employee registration failed" });
        }
    }
}

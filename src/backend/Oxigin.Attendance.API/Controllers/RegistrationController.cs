// Controller for unified registration (employees, staff, clients, etc.)
using Microsoft.AspNetCore.Mvc;
using Oxigin.Attendance.API.Abstractions;
using Oxigin.Attendance.Core.Interfaces.Managers;
using Oxigin.Attendance.Shared.Models.Entities;
using Oxigin.Attendance.Shared.Enums;

namespace Oxigin.Attendance.API.Controllers;

/// <summary>
/// Handles registration for all user types (employee, staff, client, etc.).
/// </summary>
[ApiController]
[Route("[controller]")]
public class RegistrationController : BaseController
{
    private readonly IEmployeeManager _employeeManager;
    private readonly IAdminManager _adminManager;
    // Add other managers as needed (e.g., IStaffManager, IClientManager)

    public RegistrationController(IEmployeeManager employeeManager, IAdminManager adminManager, ILogger<RegistrationController> logger)
        : base(logger)
    {
        _employeeManager = employeeManager;
        _adminManager = adminManager;
        // Inject other managers as needed
    }

    /// <summary>
    /// Register a new user (employee, staff, client, etc.).
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] UserRegistration registration, CancellationToken token)
    {
        switch (registration.UserType)
        {
            case UserType.Employee:
                // Map UserRegistration to EmployeeRegistration
                var empReg = new EmployeeRegistration
                {
                    FirstName = registration.FirstName,
                    LastName = registration.LastName,
                    Email = registration.Email,
                    Password = registration.Password,
                    // Add other mappings as needed
                };
                return Ok(await _employeeManager.RegisterAsync(empReg, token));
            case UserType.Client:
                // Map UserRegistration to Client
                var client = new Client
                {
                    Name = registration.FirstName + " " + registration.LastName,
                    Email = registration.Email,
                    // Add other mappings as needed
                };
                return Ok(await _adminManager.CreateOrUpdateClientAsync(client, token));
            default:
                return BadRequest("Unsupported user type");
        }
    }
}

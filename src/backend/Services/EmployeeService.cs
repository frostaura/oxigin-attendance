using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Models;

namespace Backend.Services
{
    public interface IEmployeeService
    {
        Task<bool> RegisterAsync(EmployeeRegistrationDto registrationDto);
        Task<string> AuthenticateAsync(EmployeeAuthenticationDto authenticationDto);
        Task<bool> SendNotificationAsync(NotificationDto notificationDto);
        Task<bool> CheckInAsync(CheckInDto checkInDto);
    }

    public class EmployeeService : IEmployeeService
    {
        private readonly List<Employee> _employees = new List<Employee>();

        public async Task<bool> RegisterAsync(EmployeeRegistrationDto registrationDto)
        {
            var employee = new Employee
            {
                Id = _employees.Count + 1,
                FirstName = registrationDto.FirstName,
                LastName = registrationDto.LastName,
                Email = registrationDto.Email,
                Password = registrationDto.Password,
                PhoneNumber = registrationDto.PhoneNumber,
                Role = registrationDto.Role,
                DateOfBirth = registrationDto.DateOfBirth,
                Address = registrationDto.Address,
                NotificationPreference = registrationDto.NotificationPreference
            };

            _employees.Add(employee);
            return await Task.FromResult(true);
        }

        public async Task<string> AuthenticateAsync(EmployeeAuthenticationDto authenticationDto)
        {
            var employee = _employees.Find(e => e.Email == authenticationDto.Email && e.Password == authenticationDto.Password);
            if (employee != null)
            {
                return await Task.FromResult("token");
            }
            return await Task.FromResult<string>(null);
        }

        public async Task<bool> SendNotificationAsync(NotificationDto notificationDto)
        {
            // Simulate sending notification
            return await Task.FromResult(true);
        }

        public async Task<bool> CheckInAsync(CheckInDto checkInDto)
        {
            var employee = _employees.Find(e => e.Id == checkInDto.EmployeeId);
            if (employee != null)
            {
                employee.CheckInTime = DateTime.Now;
                employee.Latitude = checkInDto.Latitude;
                employee.Longitude = checkInDto.Longitude;
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }
    }

    public class EmployeeRegistrationDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string NotificationPreference { get; set; }
    }

    public class EmployeeAuthenticationDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class NotificationDto
    {
        public int EmployeeId { get; set; }
        public string Message { get; set; }
    }

    public class CheckInDto
    {
        public int EmployeeId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}

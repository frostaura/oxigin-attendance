using Oxigin.Attendance.Shared.Models.Entities;

namespace Oxigin.Attendance.Core.Interfaces.Managers;

public interface IEmployeeManager
{
    Task<Employee> RegisterAsync(EmployeeRegistration registration, CancellationToken token);
    Task<Employee> AuthenticateAsync(EmployeeLogin login, CancellationToken token);
    Task<List<JobAssignment>> GetAssignedJobsAsync(CancellationToken token);
    Task<CheckInRecord> CheckInAsync(CheckInData checkIn, CancellationToken token);
    Task<List<CheckInRecord>> GetCheckInHistoryAsync(CancellationToken token);
}

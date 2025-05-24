using Oxigin.Attendance.Core.Interfaces.Managers;
using Oxigin.Attendance.Shared.Models.Entities;

namespace Oxigin.Attendance.Core.Services.Managers;

public class EmployeeManager : IEmployeeManager
{
    public Task<Employee> RegisterAsync(EmployeeRegistration registration, CancellationToken token)
    {
        // TODO: Implement logic
        return Task.FromResult(new Employee());
    }
    public Task<Employee> AuthenticateAsync(EmployeeLogin login, CancellationToken token)
    {
        // TODO: Implement logic
        return Task.FromResult(new Employee());
    }
    public Task<List<JobAssignment>> GetAssignedJobsAsync(CancellationToken token)
    {
        // TODO: Implement logic
        return Task.FromResult(new List<JobAssignment>());
    }
    public Task<CheckInRecord> CheckInAsync(CheckInData checkIn, CancellationToken token)
    {
        // TODO: Implement logic
        return Task.FromResult(new CheckInRecord());
    }
    public Task<List<CheckInRecord>> GetCheckInHistoryAsync(CancellationToken token)
    {
        // TODO: Implement logic
        return Task.FromResult(new List<CheckInRecord>());
    }
}

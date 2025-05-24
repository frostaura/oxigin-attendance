using Oxigin.Attendance.Shared.Models.Entities;

namespace Oxigin.Attendance.Core.Interfaces.Managers;

public interface IAdminManager
{
    Task<DashboardStats> GetDashboardStatsAsync(CancellationToken token);
    Task<byte[]> GenerateTimesheetAsync(TimesheetRequest request, CancellationToken token);
    Task<List<Employee>> GetEmployeesAsync(CancellationToken token);
    Task<Employee> CreateOrUpdateEmployeeAsync(Employee employee, CancellationToken token);
    Task<bool> DeleteEmployeeAsync(Guid id, CancellationToken token);
    Task<List<Client>> GetClientsAsync(CancellationToken token);
    Task<Client> CreateOrUpdateClientAsync(Client client, CancellationToken token);
    Task<bool> DeleteClientAsync(Guid id, CancellationToken token);
    Task<List<ClientRequest>> GetRequestsAsync(CancellationToken token);
    Task<bool> AllocateStaffAsync(StaffAllocation allocation, CancellationToken token);
}

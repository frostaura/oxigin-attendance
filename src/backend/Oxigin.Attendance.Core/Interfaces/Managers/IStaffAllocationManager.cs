using Oxigin.Attendance.Shared.Models.Entities;

namespace Oxigin.Attendance.Core.Interfaces.Managers;

public interface IStaffAllocationManager
{
    Task<bool> AllocateStaffToEventAsync(StaffAllocation allocation, CancellationToken token);
    Task<List<StaffAllocation>> GetAllocationsForEventAsync(Guid eventId, CancellationToken token);
    Task<bool> NotifyAllocatedEmployeesAsync(NotificationRequest notification, CancellationToken token);
}

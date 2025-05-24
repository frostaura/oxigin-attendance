using Oxigin.Attendance.Core.Interfaces.Managers;
using Oxigin.Attendance.Shared.Models.Entities;

namespace Oxigin.Attendance.Core.Services.Managers;

public class StaffAllocationManager : IStaffAllocationManager
{
    public Task<bool> AllocateStaffToEventAsync(StaffAllocation allocation, CancellationToken token)
    {
        // TODO: Implement logic
        return Task.FromResult(true);
    }
    public Task<List<StaffAllocation>> GetAllocationsForEventAsync(Guid eventId, CancellationToken token)
    {
        // TODO: Implement logic
        return Task.FromResult(new List<StaffAllocation>());
    }
    public Task<bool> NotifyAllocatedEmployeesAsync(NotificationRequest notification, CancellationToken token)
    {
        // TODO: Implement logic
        return Task.FromResult(true);
    }
}

using Oxigin.Attendance.Shared.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Oxigin.Attendance.Datastore.Interfaces;

/// <summary>
/// The datastore signature.
/// </summary>
public interface IDatastoreContext
{
    DbSet<AdditionalWorker> AdditionalWorkers { get; set; }
    DbSet<Allocation> Allocations { get; set; }
    DbSet<Client> Clients { get; set; }
    DbSet<Employee> Employees { get; set; }
    DbSet<Job> Jobs { get; set; }
    DbSet<Oxigin.Attendance.Shared.Models.Entities.Oxigin> Oxigins { get; set; }
    DbSet<Timesheet> Timesheets { get; set; }
    DbSet<User> Users { get; set; }
    DbSet<UserSession> UserSessions { get; set; }

    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

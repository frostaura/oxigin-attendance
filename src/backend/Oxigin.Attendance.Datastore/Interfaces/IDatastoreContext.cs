using Oxigin.Attendance.Shared.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Oxigin.Attendance.Datastore.Interfaces;

/// <summary>
/// The datastore signature.
/// </summary>
public interface IDatastoreContext
{
    DbSet<User> Users { get; set; }
    DbSet<UserSession> UserSessions { get; set; }
    DbSet<JobRequest> JobRequests { get; set; }

    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

using Oxigin.Attendance.Shared.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Oxigin.Attendance.Datastore.Interfaces
{
    /// <summary>
    /// The datastore signature.
    /// </summary>
    public interface IDatastoreContext
    {
        DbSet<Affiliate> Affiliates { get; set; }

        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}

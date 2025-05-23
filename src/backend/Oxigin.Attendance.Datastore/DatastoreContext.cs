using Oxigin.Attendance.Datastore.Interfaces;
using Oxigin.Attendance.Shared.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Oxigin.Attendance.Datastore
{
    /// <summary>
    /// AI Portal database context.
    /// </summary>
    public class DatastoreContext : DbContext, IDatastoreContext
    {
        /// <summary>
        /// Construct and allow for passing options.
        /// </summary>
        /// <param name="options">Db creation options.</param>
        public DatastoreContext(DbContextOptions<DatastoreContext> options)
            : base(options)
        { }

        public virtual DbSet<Affiliate> Affiliates { get; set; }
    }
}

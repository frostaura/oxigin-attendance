﻿using Oxigin.Attendance.Datastore.Interfaces;
using Microsoft.EntityFrameworkCore;
using Oxigin.Attendance.Shared.Models.Entities;

namespace Oxigin.Attendance.Datastore;

/// <summary>
/// Application database context. This is the object Entity Frameworks uses to model the database,
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
    
    public virtual DbSet<AdditionalWorker> AdditionalWorkers { get; set; }
    public virtual DbSet<Allocation> Allocations { get; set; }
    public virtual DbSet<Client> Clients { get; set; }
    public virtual DbSet<Employee> Employees { get; set; }
    public virtual DbSet<Job> Jobs { get; set; }
    public virtual DbSet<Oxigin.Attendance.Shared.Models.Entities.Oxigin> Oxigins { get; set; }
    public virtual DbSet<Timesheet> Timesheets { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<UserSession> UserSessions { get; set; }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Oxigin.Attendance.Datastore.Factories.DesignTime;

/// <summary>
/// DB context factory for running migrations in design time.
/// This allows for running migrations in the .Data project independently.
/// </summary>
public class DatastoreContextDesignTimeFactory : IDesignTimeDbContextFactory<DatastoreContext>
{
    /// <summary>
    /// Factory method for producing the design time db context
    /// </summary>
    /// <param name="args"></param>
    /// <returns>Database context.</returns>
    public DatastoreContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Migrations.json")
            .Build();
        var builder = new DbContextOptionsBuilder<DatastoreContext>();
        var connectionString = configuration
            .GetConnectionString("DatastoreConnection");

        builder.UseNpgsql(connectionString);

        Console.WriteLine($"Used connection string for configuration db: {connectionString}");

        return new DatastoreContext(builder.Options);
    }
}

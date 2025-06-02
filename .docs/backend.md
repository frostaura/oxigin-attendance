[<< Back to README.md](./../README.md)

# Backend
The backend project, developed in Dotnet 9
- Serves as the system's API.
- Allows for database comms.
- Handles auth.
- Serves static content as needed by the [frontend](./frontend.md) project.

## Database Migrations (EF Core)
### Overview
For migrations, we need to add them initially and update or re-add them each time the context changes. The actual execution of migrations happen on application start and is autonomous.

### Lessons Learnt
- In order to create migrations, the DB context has to either
    - Have a default constructor.
    - Have a IDesignTimeDbContextFactory implementation. - This allows for providing the args to the overloaded constructor during design-time as there is no DI available during that time.
    - Migration library has to be a core library and not a standard library.
    - Migration project has to reference 'Microsoft.EntityFrameworkCore.Tools'.

### Re-create Migrations (Package-Manager Console)
This covers how to add migrations with Entity Framework. This is required when any entities have been changed, added or removed. You should name your migration according to your changes, as opposed to "InitialDbMigration". This allows EF to do incremental and automated roll-outs of the database.

NOTE:
```
Migrations would need to be added again when the database provider is switched. For example switching between SQLite and Postgres. This is because different providers may have different migration features and capabilities and are rarely compatible with eachother. - Something we will likely do pre-deployment - not for local dev.
```

> dotnet tool install --global dotnet-ef

Now switch your terminal's working directory to the `src/backend/Oxigin.Attendance.Datastore` root.

> dotnet ef migrations add InitialDbMigration -c DatastoreContext -o Migrations

At this point you can view the migration code file(s) under the 'src/backend/Oxigin.Attendance.Datastore/Migrations' directory and double-check if the relationships between your entities are as expected before running the next step.

> dotnet ef database update -c DatastoreContext

### Using the DB In-code
The database context is DI-injectable via `IDatastoreContext`. For example:
A database context factory registerd and available for injection.

Resolving the DB:
```csharp
    
    /// <summary>
    /// Overloaded constructor to allow for injecting dependencies.
    /// </summary>
    /// <param name="db">The context of the database.</param>
    /// <param name="logger">The logger instance.</param>
    public UserManager(IDatastoreContext db, ILogger<UserManager> logger)
    {
        _db = db.ThrowIfNull(nameof(db));
        _logger = logger.ThrowIfNull(nameof(logger));
    }
```
Querying the DB:
```csharp
public async Task<UserSigninResponse> SignInAsync(UserSigninRequest request, CancellationToken token)
{
    request.ThrowIfNull(nameof(request));

    // Hash the originally-provided password from the user since we store that password in the DB, not the plain text one.
    var hashedPassword = request
        .Password
        .ThrowIfNullOrWhitespace(request.Password)
        .HashString();
    // Attempt to fetch a user from the DB with the matching email and hashed password. This will be null if there is no matching user.
    var user = await _db
        .Users
        .FirstOrDefaultAsync(u => u.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase) && u.Password.Equals(hashedPassword), token);

    // If there were no matching user, the sign in request failed.
    if (user == null) return new UserSigninResponse { IsSuccess = false };

    // Otherwise respond with the fetched user context.
    return new UserSigninResponse { User = user };
}
```

### Resetting the DB
On occasion, migrations may get confused and reprovisioning the DB is the easiest way. To do this, simply use your postgres extension on VS Code to connect to your Postgres server and run the following command on the DB-level.
```sql
DROP SCHEMA public CASCADE;
CREATE SCHEMA public;
```
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Oxigin.Attendance.Core.Interfaces.Managers;
using Oxigin.Attendance.Datastore.Interfaces;
using Oxigin.Attendance.Shared.Models.Entities;

namespace Oxigin.Attendance.Core.Services.Managers;

/// <summary>
/// Manager for handling Client CRUD operations.
/// </summary>
public class ClientManager : IClientManager
{
    /// <summary>
    /// The database context for accessing and persisting clients.
    /// </summary>
    private readonly IDatastoreContext _db;
    /// <summary>
    /// Logger instance for this manager.
    /// </summary>
    private readonly ILogger<ClientManager> _logger;

    /// <summary>
    /// Constructor for ClientManager.
    /// </summary>
    /// <param name="db">The database context.</param>
    /// <param name="logger">Logger instance.</param>
    public ClientManager(IDatastoreContext db, ILogger<ClientManager> logger)
    {
        _db = db;
        _logger = logger;
    }

    /// <summary>
    /// Get all clients.
    /// </summary>
    /// <param name="token">Cancellation token.</param>
    /// <returns>List of Client entities.</returns>
    public async Task<IEnumerable<Client>> GetAllAsync(CancellationToken token)
    {
        return await _db.Clients.ToListAsync(token);
    }

    /// <summary>
    /// Get a client by ID.
    /// </summary>
    /// <param name="id">The Client ID.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The Client entity, or null if not found.</returns>
    public async Task<Client?> GetByIdAsync(Guid id, CancellationToken token)
    {
        return await _db.Clients.FirstOrDefaultAsync(c => c.Id == id, token);
    }

    /// <summary>
    /// Add a new client.
    /// </summary>
    /// <param name="client">The Client entity to add.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The created Client entity.</returns>
    public async Task<Client> AddAsync(Client client, CancellationToken token)
    {
        _db.Clients.Add(client);
        await _db.SaveChangesAsync(token);
        return client;
    }

    /// <summary>
    /// Update an existing client.
    /// </summary>
    /// <param name="client">The Client entity with updated details.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The updated Client entity.</returns>
    public async Task<Client> UpdateAsync(Client client, CancellationToken token)
    {
        _db.Clients.Update(client);
        await _db.SaveChangesAsync(token);
        return client;
    }

    /// <summary>
    /// Remove a client by ID.
    /// </summary>
    /// <param name="id">The Client ID.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task RemoveAsync(Guid id, CancellationToken token)
    {
        var client = await _db.Clients.FirstOrDefaultAsync(c => c.Id == id, token);
        if (client != null)
        {
            _db.Clients.Remove(client);
            await _db.SaveChangesAsync(token);
        }
    }
}

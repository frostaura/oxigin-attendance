using Oxigin.Attendance.Shared.Models.Entities;

namespace Oxigin.Attendance.Core.Interfaces.Managers;

/// <summary>
/// Manager interface for handling Client CRUD operations.
/// </summary>
public interface IClientManager
{
    /// <summary>
    /// Get all clients.
    /// </summary>
    /// <param name="token">Cancellation token.</param>
    /// <returns>List of Client entities.</returns>
    Task<IEnumerable<Client>> GetAllAsync(CancellationToken token);

    /// <summary>
    /// Get a client by ID.
    /// </summary>
    /// <param name="id">The Client ID.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The Client entity, or null if not found.</returns>
    Task<Client?> GetByIdAsync(Guid id, CancellationToken token);

    /// <summary>
    /// Add a new client.
    /// </summary>
    /// <param name="client">The Client entity to add.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The created Client entity.</returns>
    Task<Client> AddAsync(Client client, CancellationToken token);

    /// <summary>
    /// Update an existing client.
    /// </summary>
    /// <param name="client">The Client entity with updated details.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The updated Client entity.</returns>
    Task<Client> UpdateAsync(Client client, CancellationToken token);

    /// <summary>
    /// Remove a client by ID.
    /// </summary>
    /// <param name="id">The Client ID.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task RemoveAsync(Guid id, CancellationToken token);
}

using Microsoft.AspNetCore.Mvc;
using Oxigin.Attendance.API.Abstractions;
using Oxigin.Attendance.Core.Interfaces.Managers;
using Oxigin.Attendance.Datastore.Interfaces;
using Oxigin.Attendance.Shared.Models.Entities;

namespace Oxigin.Attendance.API.Controllers;

/// <summary>
/// Controller for managing Client entities.
/// </summary>
[ApiController]
[Route("[controller]")]
public class ClientController : BaseController
{
    /// <summary>
    /// The client manager service for handling client operations.
    /// </summary>
    private readonly IClientManager _clientManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="ClientController"/> class.
    /// </summary>
    /// <param name="clientManager">The client manager service.</param>
    /// <param name="logger">The logger instance.</param>
    /// <param name="db">The database context.</param>
    public ClientController(IClientManager clientManager, ILogger<ClientController> logger, IDatastoreContext db)
        : base(logger, db)
    {
        _clientManager = clientManager;
    }

    /// <summary>
    /// Get all clients.
    /// </summary>
    /// <param name="token">Cancellation token.</param>
    /// <returns>List of Client entities.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken token)
    {
        var signedInUser = await GetRequestingUserAsync(token);
        if (signedInUser == null) return Forbid();
        var clients = await _clientManager.GetAllAsync(token);
        return Ok(clients.Where(c => !c.Deleted));
    }

    /// <summary>
    /// Get a client by ID.
    /// </summary>
    /// <param name="id">The Client ID.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The Client entity, or NotFound if not found.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken token)
    {
        var signedInUser = await GetRequestingUserAsync(token);
        if (signedInUser == null) return Forbid();
        var client = await _clientManager.GetByIdAsync(id, token);
        if (client == null || client.Deleted)
            return NotFound();
        return Ok(client);
    }

    /// <summary>
    /// Add a new client.
    /// </summary>
    /// <param name="client">The Client entity to add.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The created Client entity.</returns>
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] Client client, CancellationToken token)
    {
        var signedInUser = await GetRequestingUserAsync(token);
        if (signedInUser == null) return Forbid();
        var created = await _clientManager.AddAsync(client, token);
        return Ok(created);
    }

    /// <summary>
    /// Update an existing client.
    /// </summary>
    /// <param name="client">The Client entity with updated details.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The updated Client entity.</returns>
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] Client client, CancellationToken token)
    {
        var signedInUser = await GetRequestingUserAsync(token);
        if (signedInUser == null) return Forbid();
        var updated = await _clientManager.UpdateAsync(client, token);
        return Ok(updated);
    }

    /// <summary>
    /// Remove a client by ID.
    /// </summary>
    /// <param name="id">The Client ID.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>No content if successful.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Remove(Guid id, CancellationToken token)
    {
        var signedInUser = await GetRequestingUserAsync(token);
        if (signedInUser == null) return Forbid();
        await _clientManager.RemoveAsync(id, token);
        return NoContent();
    }
}

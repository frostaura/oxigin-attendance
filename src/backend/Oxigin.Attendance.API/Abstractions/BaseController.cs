using FrostAura.Libraries.Core.Extensions.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oxigin.Attendance.Datastore.Interfaces;
using Oxigin.Attendance.Shared.Models.Entities;

namespace Oxigin.Attendance.API.Abstractions;

/// <summary>
/// Base controller class providing logger and route setup for derived controllers.
/// </summary>
[ApiController]
[Route("[controller]")]
public abstract class BaseController : ControllerBase
{
    /// <summary>
    /// Controller logger instance.
    /// </summary>
    protected readonly ILogger logger;
    
    protected readonly IDatastoreContext datastoreContext;

    /// <summary>
    /// Overloaded constructor to allow for injecting dependencies.
    /// </summary>
    /// <param name="logger">The controller logger instance.</param>
    protected BaseController(ILogger logger, IDatastoreContext dbContext)
    {
        this.logger = logger.ThrowIfNull(nameof(logger));
        this.datastoreContext = dbContext.ThrowIfInvalid(nameof(dbContext));
    }
    
    protected async Task<User?> GetRequestingUserAsync(CancellationToken token)
    {
        var HEADER_KEY = "SessionId";

        if (!Request.Headers.ContainsKey(HEADER_KEY)) return null;
        
        // TODO: Try to grab the session id from the headers.
        var sessionId = Request.Headers[HEADER_KEY].ToString();
        
        if(sessionId is null) return null;
        
        var userContext = await datastoreContext
            .UserSessions
            .Include(s => s.User)
            .SingleAsync(s => s.Id == Guid.Parse(sessionId), token);
        
        return userContext.User;
    }
}

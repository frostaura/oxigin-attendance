using FrostAura.Libraries.Core.Extensions.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Oxigin.Attendance.Core.Extensions;
using Oxigin.Attendance.Core.Interfaces.Managers;
using Oxigin.Attendance.Datastore.Interfaces;
using Oxigin.Attendance.Shared.Exceptions;
using Oxigin.Attendance.Shared.Models.Entities;
using Oxigin.Attendance.Shared.Models.Requests;
using Oxigin.Attendance.Shared.Models.Responses;
using UnauthorizedAccessException = System.UnauthorizedAccessException;

namespace Oxigin.Attendance.Core.Services.Managers;

/// <summary>
/// The manager for handling user-related use cases.
/// </summary>
public class UserManager : IUserManager
{
    /// <summary>
    /// The context of the database.
    /// </summary>
    private readonly IDatastoreContext _db;
    /// <summary>
    /// The context of the database.
    /// </summary>
    private readonly ILogger _logger;
    
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
    
    /// <summary>
    /// Sign in a user given a sign in request object.
    /// </summary>
    /// <param name="request">The user's sign in request.</param>
    /// <param name="token">A token for cancelling downstream operations.</param>
    /// <returns>A sign in response.</returns>
    public async Task<UserSigninResponse> SignInAsync(UserSigninRequest request, CancellationToken token)
    {
        try
        {
            request.ThrowIfNull(nameof(request));

            // Hash the originally-provided password from the user since we store that password in the DB, not the plain text one.
            request.Email = request.Email.ToLower();
            var hashedPassword = request
                .Password
                .ThrowIfNullOrWhitespace(request.Password)
                .HashString();
            
            // Attempt to fetch a user from the DB with the matching email and hashed password. This will be null if there is no matching user.
            var user = await _db
                .Users
                .FirstOrDefaultAsync(u => u.Email == request.Email && u.Password == hashedPassword, token);
            
            // If there were no matching user, the sign in request failed.
            if (user == null) throw new UnauthorizedAccessException("No user found with the provided email and password.");
        
            // Create a session for the user.
            var session = new UserSession
            {
                UserId = user.Id
            };

            _db.UserSessions.Add(session);
            await _db.SaveChangesAsync(token);

            // Otherwise respond with the fetched user context.
            return new UserSigninResponse { User = user, SessionId = session.Id };
        }
        catch (Exception e)
        {
            throw new StandardizedErrorException
            {
                Error = new StandardizedError
                {
                    Origin = nameof(UserManager),
                    Message = "Unable to sign in, please try again later.",
                    Data = new Dictionary<string, object>
                    {
                        { nameof(e.Message), e.Message },
                        { nameof(e.StackTrace), e.StackTrace }
                    }
                }
            };
        }
    }

    /// <summary>
    /// Sign up a user given a user entity and get a fresh session for them.
    /// </summary>
    /// <param name="user">The user entity containing user details.</param>
    /// <param name="token">A token for cancelling downstream operations.</param>
    /// <returns>The created user's new session.</returns>
    public async Task<UserSession> SignUpAsync(User user, CancellationToken token)
    {
        try
        {
            user.ThrowIfNull(nameof(user));
        
            // Hash the password before saving
            user.Password = user.Password.HashString();
            user.Email = user.Email.ToLower();
        
            _db.Users.Add(user);
            await _db.SaveChangesAsync(token);

            var session = new UserSession { UserId = user.Id };
        
            _db.UserSessions.Add(session);
            await _db.SaveChangesAsync(token);
            return session;
        }
        catch (StandardizedErrorException e)
        {
            throw new StandardizedErrorException
            {
                Error = new StandardizedError
                {
                    Origin = nameof(UserManager),
                    Message = "Unable to sign up, please try again later.",
                    Data = new Dictionary<string, object>
                    {
                        { nameof(e.Message), e.Message },
                        { nameof(e.StackTrace), e.StackTrace }
                    }
                }
            };
        }
    }
}
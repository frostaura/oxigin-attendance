using FrostAura.Libraries.Core.Extensions.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Oxigin.Attendance.Core.Extensions;
using Oxigin.Attendance.Core.Interfaces.Data;
using Oxigin.Attendance.Core.Interfaces.Managers;
using Oxigin.Attendance.Datastore.Interfaces;
using Oxigin.Attendance.Shared.Enums;
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
    /// The notification manager for sending notifications.
    /// </summary>
    private readonly INotificationData _notificationData;
    
    /// <summary>
    /// Overloaded constructor to allow for injecting dependencies.
    /// </summary>
    /// <param name="db">The context of the database.</param>
    /// <param name="logger">The logger instance.</param>
    /// <param name="notificationData">The notification manager instance.</param>
    public UserManager(IDatastoreContext db, ILogger<UserManager> logger, INotificationData notificationData)
    {
        _db = db.ThrowIfNull(nameof(db));
        _logger = logger.ThrowIfNull(nameof(logger));
        _notificationData = notificationData.ThrowIfNull(nameof(notificationData));
    }
    
    /// <summary>
    /// Sign in a user given a sign in request object.
    /// </summary>
    /// <param name="request">The user's sign in request.</param>
    /// <param name="token">A token for cancelling downstream operations.</param>
    /// <returns>A sign in response.</returns>
    public async Task<UserSigninResponse> SignInAsync(Credentials request, CancellationToken token)
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
            // Send notification for new session/login
            await _notificationData.SendNotificationAsync(user, "A new session has started for your account.", token);
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
                        { nameof(e.StackTrace), e.StackTrace ?? string.Empty }
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
    public async Task<UserSigninResponse> SignUpAsync(User user, CancellationToken token)
    {
        try
        {
            user.ThrowIfNull(nameof(user));
        
            // Hash the password before saving
            user.Password = user.Password.HashString();
            user.Email = user.Email.ToLower();

            // If clientID is provided, verify it exists
            if (user.ClientID.HasValue)
            {
                var client = await _db.Clients.FirstOrDefaultAsync(c => c.Id == user.ClientID && !c.Deleted, token);
                if (client == null)
                {
                    throw new StandardizedErrorException
                    {
                        Error = new StandardizedError
                        {
                            Origin = nameof(UserManager),
                            Message = "The specified client does not exist.",
                            Data = new Dictionary<string, object>
                            {
                                { "ClientID", user.ClientID.Value }
                            }
                        }
                    };
                }
            }
        
            _db.Users.Add(user);
            await _db.SaveChangesAsync(token);
            var session = new UserSession { UserId = user.Id };
            _db.UserSessions.Add(session);
            await _db.SaveChangesAsync(token);
            // Send welcome notification upon registration
            await _notificationData.SendNotificationAsync(user, $"Welcome to Oxigin Attendance, {user.Name}!", token);
            return new UserSigninResponse
            {
                User = user,
                SessionId = session.Id
            };
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
                        { nameof(e.StackTrace), e.StackTrace ?? string.Empty }
                    }
                }
            };
        }
    }

    public async Task<UserSigninResponse> ChangePasswordAsync(Credentials request, CancellationToken token)
    {
        try
        {
            request.ThrowIfNull(nameof(request));
        
            // Hash the password before saving
            request.Password = request.Password.HashString();
            request.Email = request.Email.ToLower();

            var user = await _db.Users.SingleOrDefaultAsync((User) => User.Email == request.Email);
            user.Password = request.Password;
            
            await _db.SaveChangesAsync(token);
            var session = new UserSession { UserId = user.Id };
            _db.UserSessions.Add(session);
            await _db.SaveChangesAsync(token);
            return new UserSigninResponse
            {
                User = user,
                SessionId = session.Id
            };
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
                        { nameof(e.StackTrace), e.StackTrace ?? string.Empty }
                    }
                }
            };
        }
    }

    /// <summary>
    /// Refreshes the session for a signed-in user by creating a new session and returning the updated session context.
    /// </summary>
    /// <param name="user">The signed-in user entity for whom the session should be refreshed.</param>
    /// <param name="token">A token for cancelling downstream operations.</param>
    /// <returns>A <see cref="UserSigninResponse"/> containing the user and the new session ID.</returns>
    public async Task<UserSigninResponse> RefreshSessionAsync(User user, CancellationToken token)
    {
        try
        {
            user.ThrowIfNull(nameof(user));
            // Create a new session for the user
            var session = new UserSession { UserId = user.Id };

            _db.UserSessions.Add(session);
            await _db.SaveChangesAsync(token);
            return new UserSigninResponse
            {
                User = user,
                SessionId = session.Id
            };
        }
        catch (Exception e)
        {
            throw new StandardizedErrorException
            {
                Error = new StandardizedError
                {
                    Origin = nameof(UserManager),
                    Message = "Unable to refresh session, please try again later.",
                    Data = new Dictionary<string, object>
                    {
                        { nameof(e.Message), e.Message },
                        { nameof(e.StackTrace), e.StackTrace ?? string.Empty }
                    }
                }
            };
        }
    }

    /// <summary>
    /// Updates an existing user's details in the database.
    /// </summary>
    /// <param name="user">The user entity with updated details.</param>
    /// <param name="token">A token for cancelling downstream operations.</param>
    /// <returns>The updated user entity.</returns>
    public async Task<User> UpdateUserAsync(User user, CancellationToken token)
    {
        try
        {
            user.ThrowIfNull(nameof(user));
            var dbUser = await _db.Users.SingleOrDefaultAsync(u => u.Id == user.Id, token);
            if (dbUser == null) throw new StandardizedErrorException { Error = new StandardizedError { Origin = nameof(UserManager), Message = "User not found." } };

            // If clientID is provided and changed, verify it exists
            if (user.ClientID != dbUser.ClientID && user.ClientID.HasValue)
            {
                var client = await _db.Clients.FirstOrDefaultAsync(c => c.Id == user.ClientID && !c.Deleted, token);
                if (client == null)
                {
                    throw new StandardizedErrorException
                    {
                        Error = new StandardizedError
                        {
                            Origin = nameof(UserManager),
                            Message = "The specified client does not exist.",
                            Data = new Dictionary<string, object>
                            {
                                { "ClientID", user.ClientID.Value }
                            }
                        }
                    };
                }
            }

            // If employeeID is provided and changed, verify it exists
            if (user.EmployeeID != dbUser.EmployeeID && user.EmployeeID.HasValue)
            {
                var employee = await _db.Employees.FirstOrDefaultAsync(e => e.Id == user.EmployeeID && !e.Deleted, token);
                if (employee == null)
                {
                    throw new StandardizedErrorException
                    {
                        Error = new StandardizedError
                        {
                            Origin = nameof(UserManager),
                            Message = "The specified employee does not exist.",
                            Data = new Dictionary<string, object>
                            {
                                { "EmployeeID", user.EmployeeID.Value }
                            }
                        }
                    };
                }
            }

            // Handle user type changes and related foreign keys
            if (user.UserType != dbUser.UserType)
            {
                // Clear both foreign keys when changing user type
                dbUser.ClientID = null;
                dbUser.EmployeeID = null;

                // Set the appropriate foreign key based on new user type
                switch (user.UserType)
                {
                    case UserType.Client:
                        dbUser.ClientID = user.ClientID;
                        break;
                    case UserType.Employee:
                    case UserType.SiteManager:
                        dbUser.EmployeeID = user.EmployeeID;
                        break;
                }
            }
            else
            {
                // If user type hasn't changed, just update the foreign keys
                dbUser.ClientID = user.ClientID;
                dbUser.EmployeeID = user.EmployeeID;
            }

            dbUser.Name = user.Name;
            dbUser.ContactNr = user.ContactNr;
            dbUser.Email = user.Email;
            dbUser.UserType = user.UserType;
            // Do not update password here for security reasons
            await _db.SaveChangesAsync(token);
            return dbUser;
        }
        catch (Exception e)
        {
            throw new StandardizedErrorException
            {
                Error = new StandardizedError
                {
                    Origin = nameof(UserManager),
                    Message = "Failed to update user.",
                    Data = new Dictionary<string, object>
                    {
                        { "UserId", user.Id },
                        { "Error", e.Message }
                    }
                }
            };
        }
    }

}
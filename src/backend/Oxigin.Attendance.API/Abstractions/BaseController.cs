// Abstract base controller for all API controllers in Oxigin Attendance.
// Provides logger injection and common controller setup.

using FrostAura.Libraries.Core.Extensions.Validation;
using Microsoft.AspNetCore.Mvc;

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

    /// <summary>
    /// Overloaded constructor to allow for injecting dependencies.
    /// </summary>
    /// <param name="logger">The controller logger instance.</param>
    public BaseController(ILogger logger)
    {
        this.logger = logger.ThrowIfNull(nameof(logger));
    }
}

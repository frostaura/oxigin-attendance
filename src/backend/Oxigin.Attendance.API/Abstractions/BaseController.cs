using Microsoft.AspNetCore.Mvc;
using Oxigin.Attendance.Core.Extensions;

namespace Oxigin.Attendance.API.Abstractions;

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

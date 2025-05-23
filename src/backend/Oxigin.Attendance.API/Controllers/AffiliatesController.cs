using Oxigin.Attendance.API.Abstractions;
using Oxigin.Attendance.Core.Extensions;
using Oxigin.Attendance.Core.Interfaces.Managers;
using Oxigin.Attendance.Shared.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Oxigin.Attendance.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AffiliatesController : BaseController
{
    /// <summary>
    /// A manager to fascilitate for Affiliate-related use cases.
    /// </summary>
    private readonly IAffiliateManager _affiliateManager;

    /// <summary>
    /// Overloaded constructor to allow for injecting dependencies.
    /// </summary>
    /// <param name="affiliateManager">A manager to fascilitate for Affiliate-related use cases.</param>
    /// <param name="logger">The controller logger instance.</param>
    public AffiliatesController(IAffiliateManager affiliateManager, ILogger<AffiliatesController> logger)
        :base(logger)
    {
        _affiliateManager = affiliateManager.ThrowIfNull(nameof(affiliateManager));
    }

    /// <summary>
    /// Get all currently-active / non-deleted affiliates.
    /// </summary>
    /// <param name="token">Token to cancel downstream operations.</param>
    /// <returns>A collection of active affiliates.</returns>
    [HttpGet(Name = "GetAllActiveAffiliatesAsync")]
    public Task<List<Affiliate>> GetAsync(CancellationToken token)
    {
        return _affiliateManager.GetActiveAffiliatesAsync(token);
    }
}


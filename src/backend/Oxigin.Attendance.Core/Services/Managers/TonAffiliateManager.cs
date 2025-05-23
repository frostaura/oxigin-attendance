using Oxigin.Attendance.Core.Extensions;
using Oxigin.Attendance.Core.Interfaces.Managers;
using Oxigin.Attendance.Datastore.Interfaces;
using Oxigin.Attendance.Shared.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Oxigin.Attendance.Core.Services.Managers;

public class TonAffiliateManager : IAffiliateManager
{
  /// <summary>
  /// The datastore context instance.
  /// </summary>
  private readonly IDatastoreContext _datastoreContext;
  /// <summary>
  /// 
  /// </summary>
  private readonly ILogger<TonAffiliateManager> _logger;

  /// <summary>
  /// Overloaded constructor to allow for injecting dependencies.
  /// </summary>
  /// <param name="datastoreContext">The datastore context instance.</param>
  /// <param name="logger">The logger instance.</param>
  public TonAffiliateManager(IDatastoreContext datastoreContext, ILogger<TonAffiliateManager> logger)
  {
    _datastoreContext = datastoreContext.ThrowIfNull(nameof(datastoreContext));
    _logger = logger;
  }

  /// <summary>
  /// Get all currently-active / non-deleted affiliates.
  /// </summary>
  /// <param name="token">Token to cancel downstream operations.</param>
  /// <returns>A collection of active affiliates.</returns>
  public Task<List<Affiliate>> GetActiveAffiliatesAsync(CancellationToken token)
  {
    return _datastoreContext
        .Affiliates
        .Where(a => !a.Deleted)
        .ToListAsync(token);
  }
}
using Oxigin.Attendance.Shared.Models.Entities;

namespace Oxigin.Attendance.Core.Interfaces.Managers;

/// <summary>
/// A manager to fascilitate for Affiliate-related use cases.
/// </summary>
public interface IAffiliateManager
{
    /// <summary>
    /// Get all active affiliates.
    /// </summary>
    /// <param name="token">A token to allow for cancelling downstream operations.</param>
    /// <returns>A collection of currently-active affiliates.</returns>
    Task<List<Affiliate>> GetActiveAffiliatesAsync(CancellationToken token);
}

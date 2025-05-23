using Oxigin.Attendance.Core.Interfaces.Managers;
using Oxigin.Attendance.Shared.Models.Configs;
using LazyCache;
using Microsoft.Extensions.Options;

namespace Oxigin.Attendance.Core.Services.Managers;

/// <summary>
/// <inheritdoc cref="IMemoryCacheManager"/>
/// </summary>
public class MemoryCacheManager : IMemoryCacheManager
{
  private readonly IAppCache _cache;
  private readonly LazyCacheConfig _lazyCacheOptions;

  /// <summary>
  ///   Dependency injection constructor.
  /// </summary>
  public MemoryCacheManager(IAppCache cache, IOptions<LazyCacheConfig> lazyCacheOptions)
  {
    _cache = cache;
    _lazyCacheOptions = lazyCacheOptions.Value;
  }

  /// <summary>
  /// <inheritdoc cref="IMemoryCacheManager.GetOrSetAsync{T}(string, Func{Task{T}}, int?)"/>
  /// </summary>
  public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> actionMethod, int? cacheDurationInSeconds = null)
  {
    return await _cache.GetOrAddAsync(
      key,
      async () => await actionMethod(),
      new TimeSpan(0, 0, cacheDurationInSeconds ?? _lazyCacheOptions.CacheDurationInSeconds));
  }

  /// <summary>
  /// <inheritdoc cref="IMemoryCacheManager.Add{T}(string, T, int?)"/>
  /// </summary>
  public void Add<T>(string key, T item, int? cacheDurationInSeconds = null)
  {
    _cache.Add(
      key,
      item,
      new TimeSpan(0, 0, cacheDurationInSeconds ?? _lazyCacheOptions.CacheDurationInSeconds));
  }

  /// <summary>
  /// <inheritdoc cref="IMemoryCacheManager.Remove(string)"/>
  /// </summary>
  public void Remove(string key)
  {
    _cache.Remove(key);
  }

  /// <summary>
  /// <inheritdoc cref="IMemoryCacheManager.TryGetValue{T}(string, out T)"/>
  /// </summary>
  public bool TryGetValue<T>(string key, out T value)
  {
    return _cache.TryGetValue(key, out value);
  }
}
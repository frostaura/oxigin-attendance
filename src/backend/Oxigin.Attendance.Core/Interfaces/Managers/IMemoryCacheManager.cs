namespace Oxigin.Attendance.Core.Interfaces.Managers;

/// <summary>
///   Memory caching manager to cache retrieval of data.
/// </summary>
public interface IMemoryCacheManager
{
  /// <summary>
  ///   Get data and cache method.
  /// </summary>
  /// <param name="key">The key identifier.</param>
  /// <param name="actionMethod">The method to cache.</param>
  /// <param name="cacheDurationInSeconds">The cache duration in seconds override.</param>
  /// <returns>Returns data result.</returns>
  Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> actionMethod, int? cacheDurationInSeconds = null);

  /// <summary>
  ///   Adds item to cache.
  /// </summary>
  /// <param name="key">The key identifier.</param>
  /// <param name="item">The item to cache.</param>
  /// <param name="cacheDurationInSeconds">The cache duration in seconds override.</param>
  /// <returns>Returns data result.</returns>
  void Add<T>(string key, T item, int? cacheDurationInSeconds = null);

  /// <summary>
  ///   Removes item from cache.
  /// </summary>
  /// <param name="key">The key identifier.</param>
  void Remove(string key);

  /// <summary>
  ///   Attempts to get the item from cache if it exists.
  /// </summary>
  /// <param name="key">The key identifier.</param>
  /// <param name="value">The value of cached data.</param>
  bool TryGetValue<T>(string key, out T value);
}
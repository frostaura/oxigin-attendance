using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Oxigin.Attendance.Core.Interfaces.Managers;
using Oxigin.Attendance.Core.Services.Managers;
using Oxigin.Attendance.Datastore.Extensions;

namespace Oxigin.Attendance.Core.Extensions;

/// <summary>
/// Extensions for IServiceCollection.
/// </summary>
public static class ServiceCollectionExtensions
{
  /// <summary>
  /// Add all required services.
  /// </summary>
  /// <param name="serviceCollection">The application service collection.</param>
  /// <param name="config">Application configuration.</param>
  /// <returns>The application service collection for allowing the Fluent usage.</returns>
  public static IServiceCollection AddCoreServices(this IServiceCollection serviceCollection, ConfigurationManager config)
  {
    var services = serviceCollection
        .AddHttpClient()
        .AddConfigs(config)
        .AddDatastoreServices(config)
        .AddServices(config);

    return services;
  }

  /// <summary>
  /// Add configs.
  /// </summary>
  /// <param name="serviceCollection">The application service collection.</param>
  /// <param name="config">Application configuration.</param>
  /// <returns>The application service collection for allowing the Fluent usage.</returns>
  private static IServiceCollection AddConfigs(this IServiceCollection serviceCollection, ConfigurationManager config)
  {
    // serviceCollection.Configure<LazyCacheConfig>(config.GetSection(LazyCacheConfig.Option));
    return serviceCollection;
  }

  /// <summary>
  /// Add all required services for the Okta integration to function.
  /// </summary>
  /// <param name="serviceCollection">The application service collection.</param>
  /// <param name="config">Application configuration.</param>
  /// <returns>The application service collection for allowing the Fluent usage.</returns>
  private static IServiceCollection AddServices(this IServiceCollection serviceCollection, ConfigurationManager config)
  {
    return serviceCollection
      .AddSingleton<IUserManager, UserManager>();
  }
}

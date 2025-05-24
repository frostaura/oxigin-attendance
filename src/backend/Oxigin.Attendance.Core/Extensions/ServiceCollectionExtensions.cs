using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Oxigin.Attendance.Shared.Models.Config;
using Oxigin.Attendance.Core.Interfaces.Managers;
using Oxigin.Attendance.Datastore.Extensions;
using Oxigin.Attendance.Core.Services.Managers;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Oxigin.Attendance.Shared.Models.Configs;

namespace Oxigin.Attendance.Core.Extensions
{
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
          .AddDatastoreServices(config)
          .AddUtilities(config)
          .AddServices(config);

      return services;
    }

    /// <summary>
    /// Add utilities.
    /// </summary>
    /// <param name="serviceCollection">The application service collection.</param>
    /// <param name="config">Application configuration.</param>
    /// <returns>The application service collection for allowing the Fluent usage.</returns>
    private static IServiceCollection AddUtilities(this IServiceCollection serviceCollection, ConfigurationManager config)
    {
      serviceCollection.Configure<LazyCacheConfig>(config.GetSection(LazyCacheConfig.Option));
      serviceCollection.AddLazyCache();
      return serviceCollection;
    }

    /// <summary>
    /// Add configs.
    /// </summary>
    /// <param name="serviceCollection">The application service collection.</param>
    /// <param name="config">Application configuration.</param>
    /// <returns>The application service collection for allowing the Fluent usage.</returns>
    private static IServiceCollection AddConfigs(this IServiceCollection serviceCollection, ConfigurationManager config)
    {
      serviceCollection.Configure<LazyCacheConfig>(config.GetSection(LazyCacheConfig.Option));
      serviceCollection.AddLazyCache();
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
          .Configure<ApplicationConfig>(config.GetSection("ApplicationConfig"))
          .Configure<MerchantConfig>(config.GetSection(MerchantConfig.Option));
    }
  }
}
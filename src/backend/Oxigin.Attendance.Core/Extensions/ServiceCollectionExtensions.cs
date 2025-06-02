using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Oxigin.Attendance.Core.Interfaces.Managers;
using Oxigin.Attendance.Core.Services.Managers;
using Oxigin.Attendance.Datastore.Extensions;
using Oxigin.Attendance.Core.Interfaces.Data;
using EmailNotificationsData = Oxigin.Attendance.Core.Services.Data.EmailNotificationsData;
using Oxigin.Attendance.Shared.Models.Config;

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
    serviceCollection.Configure<SmtpConfig>(config.GetSection("Smtp"));
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
      .AddScoped<IUserManager, UserManager>()
      .AddScoped<INotificationData, EmailNotificationsData>()
      .AddScoped<IJobRequestManager, JobManager>()
      .AddScoped<IAdditionalWorkerManager, AdditionalWorkerManager>()
      .AddScoped<IEmployeeManager, EmployeeManager>()
      .AddScoped<IClientManager, ClientManager>()
      .AddScoped<IJobAllocationManager, JobAllocationManager>();
  }
}

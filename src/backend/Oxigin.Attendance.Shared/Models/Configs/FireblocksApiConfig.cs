namespace Oxigin.Attendance.Shared.Models.Configs;

/// <summary>
/// The Fireblocks Api settings configuration class to map to the appsettings.json file.
/// </summary>
public class FireblocksApiConfig
{
  /// <summary>
  /// Defines the name of the configuration section.
  /// </summary>
  public const string Option = "FireblocksApiConfig";

  /// <summary>
  /// The base API URL.
  /// </summary>
  public string BaseUrl { get; set; }

  /// <summary>
  /// 
  /// </summary>
  public string ApiKey { get; set; }

  /// <summary>
  /// 
  /// </summary>
  public string PrivateKey { get; set; }
}

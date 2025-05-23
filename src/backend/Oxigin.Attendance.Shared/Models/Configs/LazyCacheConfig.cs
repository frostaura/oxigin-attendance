namespace Oxigin.Attendance.Shared.Models.Configs;

/// <summary>
/// The LazyCache settings configuration class to map to the appsettings.json file.
/// </summary>
public class LazyCacheConfig
{
    /// <summary>
    /// Defines the name of the configuration section.
    /// </summary>
    public const string Option = "LazyCacheConfig";

    /// <summary>
    /// The cache duration in seconds before cache is reset.
    /// </summary>
    public int CacheDurationInSeconds { get; set; }
}
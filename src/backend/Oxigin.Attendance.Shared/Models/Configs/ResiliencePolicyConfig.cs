namespace Oxigin.Attendance.Shared.Models.Configs;

/// <summary>
/// The Resilience Policy configuration class to get settings from appsettings.json file.
/// </summary>
public class ResiliencePolicyConfig
{
    /// <summary>
    /// Defines the name of the configuration section.
    /// </summary>
    public const string Option = "ResiliencePolicyConfig";

    /// <summary>
    /// The duration until circuit is closed again.
    /// </summary>
    public int CircuitBreakDurationInSeconds { get; set; }

    /// <summary>
    /// The attempts before circuit breaks.
    /// </summary>
    public int AllowedBeforeBreak { get; set; }

    /// <summary>
    /// Retry when error in thrown.
    /// </summary>
    public int NumberOfRetries { get; set; }

    /// <summary>
    /// Delay between retries using de-correlated jitter backoff.
    /// </summary>
    public int MedianFirstRetryDelayInSeconds { get; set; }
}
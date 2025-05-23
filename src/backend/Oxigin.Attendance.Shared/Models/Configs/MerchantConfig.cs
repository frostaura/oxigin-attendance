namespace Oxigin.Attendance.Shared.Models.Configs;

/// <summary>
/// The Merchant Info settings configuration class to map to the appsettings.json file.
/// </summary>
public class MerchantConfig
{
    /// <summary>
    /// Defines the name of the configuration section.
    /// </summary>
    public const string Option = "MerchantConfig";

    /// <summary>
    /// The wallet address.
    /// </summary>
    public string WalletAddress { get; set; }
}
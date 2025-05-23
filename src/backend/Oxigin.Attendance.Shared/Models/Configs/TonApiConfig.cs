namespace Oxigin.Attendance.Shared.Models.Configs;

/// <summary>
/// The Ton Api settings configuration class to map to the appsettings.json file.
/// </summary>
public class TonApiConfig
{
    /// <summary>
    /// Defines the name of the configuration section.
    /// </summary>
    public const string Option = "TonApiConfig";

    /// <summary>
    /// The cache duration in seconds before cache is reset.
    /// </summary>
    public string BaseUrl { get; set; }

    /// <summary>
    /// The base api uri.
    /// </summary>
    public string BaseApiUri { get; set; }
  
    /// <summary>
    /// The URL of the HTTP endpoint to call the contract get methods.
    /// </summary>
    public string GetterUrl { get; set; }

    /// <summary>
    /// The address of the smart contract to use as the state store.
    /// </summary>
    public string SmartContractAddress { get; set; }

    /// <summary>
    /// The recovery phrase for the gas station account.
    /// </summary>
    public List<string> GasStationMnemonicPhrase { get; set; }
}
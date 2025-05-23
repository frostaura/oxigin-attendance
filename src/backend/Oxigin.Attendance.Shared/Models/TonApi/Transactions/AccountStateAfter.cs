using System.Text.Json.Serialization;

namespace Oxigin.Attendance.Shared.Models.TonApi.Transactions;

public class AccountStateAfter
{
  [JsonPropertyName("hash")]
  public string Hash { get; set; }

  [JsonPropertyName("balance")]
  public string Balance { get; set; }

  [JsonPropertyName("account_status")]
  public string AccountStatus { get; set; }

  [JsonPropertyName("frozen_hash")]
  public object FrozenHash { get; set; }

  [JsonPropertyName("data_hash")]
  public string DataHash { get; set; }

  [JsonPropertyName("code_hash")]
  public string CodeHash { get; set; }
}

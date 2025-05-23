using System.Text.Json.Serialization;

namespace Oxigin.Attendance.Shared.Models.TonApi.Transactions;

public class Description
{
  [JsonPropertyName("type")]
  public string Type { get; set; }

  [JsonPropertyName("aborted")]
  public bool Aborted { get; set; }

  [JsonPropertyName("destroyed")]
  public bool Destroyed { get; set; }

  [JsonPropertyName("credit_first")]
  public bool CreditFirst { get; set; }

  [JsonPropertyName("storage_ph")]
  public StoragePh StoragePh { get; set; }

  [JsonPropertyName("compute_ph")]
  public ComputePh ComputePh { get; set; }

  [JsonPropertyName("action")]
  public Action Action { get; set; }
}

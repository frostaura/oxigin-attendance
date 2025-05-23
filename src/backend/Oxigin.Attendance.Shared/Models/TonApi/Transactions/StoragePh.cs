using System.Text.Json.Serialization;

namespace Oxigin.Attendance.Shared.Models.TonApi.Transactions;

public class StoragePh
{
  [JsonPropertyName("storage_fees_collected")]
  public string StorageFeesCollected { get; set; }

  [JsonPropertyName("status_change")]
  public string StatusChange { get; set; }
}

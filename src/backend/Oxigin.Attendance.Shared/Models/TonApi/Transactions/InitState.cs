using System.Text.Json.Serialization;

namespace Oxigin.Attendance.Shared.Models.TonApi.Transactions;

public class InitState
{
  [JsonPropertyName("hash")]
  public string Hash { get; set; }

  [JsonPropertyName("body")]
  public string Body { get; set; }

  [JsonPropertyName("decoded")]
  public object Decoded { get; set; }
}

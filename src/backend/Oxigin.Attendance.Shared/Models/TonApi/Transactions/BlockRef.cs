using System.Text.Json.Serialization;

namespace Oxigin.Attendance.Shared.Models.TonApi.Transactions;

public class BlockRef
{
  [JsonPropertyName("workchain")]
  public int Workchain { get; set; }

  [JsonPropertyName("shard")]
  public string Shard { get; set; }

  [JsonPropertyName("seqno")]
  public int Seqno { get; set; }
}

using System.Text.Json.Serialization;

namespace Oxigin.Attendance.Shared.Models.TonApi.Transactions;

public class TotMsgSize
{
  [JsonPropertyName("cells")]
  public string Cells { get; set; }

  [JsonPropertyName("bits")]
  public string Bits { get; set; }
}

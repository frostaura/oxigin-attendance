using System.Text.Json.Serialization;

namespace Oxigin.Attendance.Shared.Models.TonApi.Transactions;

public class OutMsgs
{
  [JsonPropertyName("hash")]
  public string Hash { get; set; }

  [JsonPropertyName("source")]
  public string Source { get; set; }

  [JsonPropertyName("destination")]
  public string Destination { get; set; }

  [JsonPropertyName("value")]
  public string? Value { get; set; }

  [JsonPropertyName("fwd_fee")]
  public string FwdFee { get; set; }

  [JsonPropertyName("ihr_fee")]
  public string IhrFee { get; set; }

  [JsonPropertyName("created_lt")]
  public string CreatedLt { get; set; }

  [JsonPropertyName("created_at")]
  public string CreatedAt { get; set; }

  [JsonPropertyName("opcode")]
  public string Opcode { get; set; }

  [JsonPropertyName("ihr_disabled")]
  public bool IhrDisabled { get; set; }

  [JsonPropertyName("bounce")]
  public bool Bounce { get; set; }

  [JsonPropertyName("bounced")]
  public bool Bounced { get; set; }

  [JsonPropertyName("import_fee")]
  public object ImportFee { get; set; }

  [JsonPropertyName("message_content")]
  public MessageContent MessageContent { get; set; }

  [JsonPropertyName("init_state")]
  public InitState InitState { get; set; }
}

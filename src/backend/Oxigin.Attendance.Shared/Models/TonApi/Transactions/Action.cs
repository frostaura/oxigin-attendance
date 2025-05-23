using System.Text.Json.Serialization;

namespace Oxigin.Attendance.Shared.Models.TonApi.Transactions;

public class Action
{
  [JsonPropertyName("success")]
  public bool Success { get; set; }

  [JsonPropertyName("valid")]
  public bool Valid { get; set; }

  [JsonPropertyName("no_funds")]
  public bool NoFunds { get; set; }

  [JsonPropertyName("status_change")]
  public string StatusChange { get; set; }

  [JsonPropertyName("total_fwd_fees")]
  public string TotalFwdFees { get; set; }

  [JsonPropertyName("total_action_fees")]
  public string TotalActionFees { get; set; }

  [JsonPropertyName("result_code")]
  public int ResultCode { get; set; }

  [JsonPropertyName("tot_actions")]
  public int TotActions { get; set; }

  [JsonPropertyName("spec_actions")]
  public int SpecActions { get; set; }

  [JsonPropertyName("skipped_actions")]
  public int SkippedActions { get; set; }

  [JsonPropertyName("msgs_created")]
  public int MsgsCreated { get; set; }

  [JsonPropertyName("action_list_hash")]
  public string ActionListHash { get; set; }

  [JsonPropertyName("tot_msg_size")]
  public TotMsgSize TotMsgSize { get; set; }
}

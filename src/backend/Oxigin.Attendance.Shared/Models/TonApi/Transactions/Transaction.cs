using System.Text.Json.Serialization;

namespace Oxigin.Attendance.Shared.Models.TonApi.Transactions;

public class Transaction
{
  [JsonPropertyName("account")]
  public string Account { get; set; }

  [JsonPropertyName("hash")]
  public string Hash { get; set; }

  [JsonPropertyName("lt")]
  public string Lt { get; set; }

  [JsonPropertyName("now")]
  public int Now { get; set; }

  [JsonPropertyName("mc_block_seqno")]
  public int McBlockSeqno { get; set; }

  [JsonPropertyName("trace_id")]
  public string TraceId { get; set; }

  [JsonPropertyName("prev_trans_hash")]
  public string PrevTransHash { get; set; }

  [JsonPropertyName("prev_trans_lt")]
  public string PrevTransLt { get; set; }

  [JsonPropertyName("orig_status")]
  public string OrigStatus { get; set; }

  [JsonPropertyName("end_status")]
  public string EndStatus { get; set; }

  [JsonPropertyName("total_fees")]
  public string TotalFees { get; set; }

  [JsonPropertyName("description")]
  public Description Description { get; set; }

  [JsonPropertyName("block_ref")]
  public BlockRef BlockRef { get; set; }

  [JsonPropertyName("in_msg")]
  public InMsg? InMsg { get; set; }

  [JsonPropertyName("out_msgs")]
  public OutMsgs[]? OutMsgs { get; set; }

  [JsonPropertyName("account_state_before")]
  public AccountStateBefore AccountStateBefore { get; set; }

  [JsonPropertyName("account_state_after")]
  public AccountStateAfter AccountStateAfter { get; set; }
}
using System.Text.Json.Serialization;

namespace Oxigin.Attendance.Shared.Models.TonApi.Transactions;

public class ComputePh
{
  [JsonPropertyName("skipped")]
  public bool Skipped { get; set; }

  [JsonPropertyName("success")]
  public bool Success { get; set; }

  [JsonPropertyName("msg_state_used")]
  public bool MsgStateUsed { get; set; }

  [JsonPropertyName("account_activated")]
  public bool AccountActivated { get; set; }

  [JsonPropertyName("gas_fees")]
  public string GasFees { get; set; }

  [JsonPropertyName("gas_used")]
  public string GasUsed { get; set; }

  [JsonPropertyName("gas_limit")]
  public string GasLimit { get; set; }

  [JsonPropertyName("gas_credit")]
  public string GasCredit { get; set; }

  [JsonPropertyName("mode")]
  public int Mode { get; set; }

  [JsonPropertyName("exit_code")]
  public int ExitCode { get; set; }

  [JsonPropertyName("vm_steps")]
  public int VmSteps { get; set; }

  [JsonPropertyName("vm_init_state_hash")]
  public string VmInitStateHash { get; set; }

  [JsonPropertyName("vm_final_state_hash")]
  public string VmFinalStateHash { get; set; }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Oxigin.Attendance.Shared.Models.FireblocksApi.Transactions
{
  public class PayoutTransactionResponse
  {
    [JsonPropertyName("payoutId")]
    [Required]
    public string PayoutId { get; set; }

    [JsonPropertyName("paymentAccount")]
    [Required]
    public Account PaymentAccounts { get; set; }

    [JsonPropertyName("createdAt")]
    [Required]
    public string CreatedAt { get; set; }

    [JsonPropertyName("state")]
    [Required]
    public string State { get; set; }

    [JsonPropertyName("status")]
    [Required]
    public string Status { get; set; }

    [JsonPropertyName("reasonOfFailure")]
    public string ReasonOfFailure { get; set; }

    [JsonPropertyName("initMethod")]
    public string InitMethod { get; set; }

    [JsonPropertyName("instructionSet")]
    [Required]
    public IEnumerable<InstructionSetResponse> InstructionSet { get; set; } = [];

    [JsonPropertyName("reportUrl")]
    [Required]
    public string ReportUrl { get; set; }
  }
}

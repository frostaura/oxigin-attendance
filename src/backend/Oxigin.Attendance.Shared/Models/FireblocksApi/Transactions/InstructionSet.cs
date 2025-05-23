using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Oxigin.Attendance.Shared.Models.FireblocksApi.Transactions
{
  public class InstructionSet
  {
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("payeeAccount")]
    [Required]
    public Account PayeeAccount { get; set; }

    [JsonPropertyName("amount")]
    [Required]
    public TransactionAmount Amount { get; set; }
  }
}

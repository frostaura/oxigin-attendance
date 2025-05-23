using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Oxigin.Attendance.Shared.Models.FireblocksApi.Transactions
{
  public class InstructionSetResponse : InstructionSet
  {
    [JsonPropertyName("state")]
    [Required]
    public string State { get; set; }

    [JsonPropertyName("transactions")]
    [Required]
    public IEnumerable<Transaction> Transactions { get; set; }
  }
}

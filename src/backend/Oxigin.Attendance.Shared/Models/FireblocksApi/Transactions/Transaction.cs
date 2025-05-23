using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Oxigin.Attendance.Shared.Models.FireblocksApi.Transactions
{
  public class Transaction
  {
    [JsonPropertyName("id")]
    [Required]
    public string Id { get; set; }

    [JsonPropertyName("state")]
    [Required]
    public string State { get; set; }

    [JsonPropertyName("timestamp")]
    public string Timestamp { get; set; }

    [JsonPropertyName("instructionId")]
    public string InstructionId { get; set; }
  }
}

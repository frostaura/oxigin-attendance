using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Oxigin.Attendance.Shared.Models.FireblocksApi.Transactions
{
  public class Account
  {
    [JsonPropertyName("id")]
    [Required]
    public string Id { get; set; }

    [JsonPropertyName("type")]
    [Required]
    public string Type { get; set; }
  }
}

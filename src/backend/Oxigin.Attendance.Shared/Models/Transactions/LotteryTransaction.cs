using Oxigin.Attendance.Shared.Models.Entities;
using System.Text.Json.Serialization;

namespace Oxigin.Attendance.Shared.Models.Transactions;

public class LotteryTransaction
{
  [JsonPropertyName("e")]
  public IEnumerable<LotteryEntry> LotteryEntries { get; set; }

  public decimal Amount { get; set; }
  
  public DateTime Timestamp { get; set; }
}
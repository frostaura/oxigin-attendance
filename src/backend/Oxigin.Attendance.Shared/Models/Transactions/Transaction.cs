using Oxigin.Attendance.Shared.Enums;
using System.Text.Json.Serialization;

namespace Oxigin.Attendance.Shared.Models.Transactions;

public abstract class Transaction
{
  [JsonPropertyName("sourceAddress")]
  public string SourceAddress { get; set; }

  [JsonPropertyName("destinationAddress")]
  public string DestinationAddress { get; set; }

  [JsonPropertyName("comment")]
  public string? Comment { get; set; }

  [JsonPropertyName("amount")]
  public decimal Amount { get; set; }

  [JsonPropertyName("type")]
  public TransactionType Type { get; set; }

  [JsonPropertyName("timestamp")]
  public DateTime Timestamp { get; set; }
}
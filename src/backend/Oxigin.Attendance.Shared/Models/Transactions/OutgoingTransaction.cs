using System.Text.Json.Serialization;

namespace Oxigin.Attendance.Shared.Models.Transactions;

public class OutgoingTransaction : Transaction
{
  [JsonPropertyName("comment")]
  public new string? Comment { get; set; }
}
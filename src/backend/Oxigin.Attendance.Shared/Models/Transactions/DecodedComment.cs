namespace Oxigin.Attendance.Shared.Models.Transactions;

using System.Text.Json.Serialization;

public class DecodedComment
{
  [JsonPropertyName("type")]
  public string Type { get; set; }

  [JsonPropertyName("comment")]
  public string Comment { get; set; }
}
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Oxigin.Attendance.Shared.Models.Transactions;

public class IncomingTransaction : Transaction
{
  [JsonPropertyName("comment")]
  public TransactionComment? TransactionComment
  {
    get
    {
      try
      {
        return Comment != null ? JsonSerializer.Deserialize<TransactionComment>(Comment) : null;
      }
      catch (JsonException)
      {
        return null;
      }
    }
  }
}
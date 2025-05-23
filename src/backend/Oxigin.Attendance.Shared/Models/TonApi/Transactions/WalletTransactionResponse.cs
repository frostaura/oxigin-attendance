using System.Text.Json.Serialization;

namespace Oxigin.Attendance.Shared.Models.TonApi.Transactions;

public class WalletTransactionResponse
{
  [JsonPropertyName("transactions")]
  public IEnumerable<Transaction> Transactions { get; set; } = [];
}

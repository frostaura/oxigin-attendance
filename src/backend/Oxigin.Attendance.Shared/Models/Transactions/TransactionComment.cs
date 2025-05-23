using Oxigin.Attendance.Shared.Enums;
using System.Text.Json.Serialization;

namespace Oxigin.Attendance.Shared.Models.Transactions;

public class TransactionComment 
{
  [JsonPropertyName("a")]
  public MainApplication MainApplication { get; set; }

  [JsonPropertyName("t")]
  public TransactionType TransactionType { get; set; }

  [JsonPropertyName("v")]
  public LotteryTransaction Value { get; set; }

  [JsonPropertyName("aid")]
  public string Aid { get; set; }

  [JsonPropertyName("affiliateId")]
  public int? AffiliateId
  {
    get
    {
      if (int.TryParse(Aid, out var affiliateId))
      {
        return affiliateId;
      }
  
      return null;
    }
  }
}


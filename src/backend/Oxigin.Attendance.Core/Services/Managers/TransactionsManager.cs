using System.Text.Json;
using Oxigin.Attendance.Core.Interfaces.Gateways;
using Oxigin.Attendance.Core.Interfaces.Managers;
using Oxigin.Attendance.Shared.Enums;
using Oxigin.Attendance.Shared.Models.Transactions;
using Microsoft.Extensions.Logging;
using Transaction = Oxigin.Attendance.Shared.Models.Transactions.Transaction;

namespace Oxigin.Attendance.Core.Services.Managers;

public class TransactionsManager : ITransactionsManager
{
  /// <summary>
  /// 
  /// </summary>
  private readonly ILogger<TransactionsManager> _logger;
  /// <summary>
  /// 
  /// </summary>
  private readonly ITonApiGateway _tonApiGateway;

  public TransactionsManager(
    ILogger<TransactionsManager> logger,
    ITonApiGateway tonApiGateway)
  {
    _logger = logger;
    _tonApiGateway = tonApiGateway;
  }

  public async Task<IEnumerable<Transaction>> GetAllTransactionsForWalletAccount(string? account, DateTime? startDate)
  {
    if (string.IsNullOrWhiteSpace(account))
    {
      throw new ArgumentNullException(nameof(account), "Account must be provided.");
    }
    
    if (startDate is null)
    {
      var now = DateTime.UtcNow;
      var lastMonday = now.AddDays(-(int)now.DayOfWeek + (int)DayOfWeek.Monday).Date;
      startDate = new DateTime(lastMonday.Year, lastMonday.Month, lastMonday.Day, 0, 0, 0, DateTimeKind.Utc);
    }

    var startTime = new DateTimeOffset((DateTime)startDate).ToUnixTimeSeconds();
    return FilterTransactions(await _tonApiGateway.GetTransactionsForWalletAccount(account, startTime));
  }

  public async Task<IEnumerable<IncomingTransaction>> GetAllIncomingTransactionsForWalletAccount(string? account, DateTime? startDate)
  {
    var allTransactions = await GetAllTransactionsForWalletAccount(account, startDate);
    return (from transaction in allTransactions
      where transaction.Type == TransactionType.Purchase
      select new IncomingTransaction
      {
        Amount = transaction.Amount,
        Comment = transaction.Comment,
        DestinationAddress = transaction.DestinationAddress,
        SourceAddress = transaction.SourceAddress,
        Timestamp = transaction.Timestamp,
        Type = transaction.Type
      }).ToList();
  }

  public async Task<IEnumerable<OutgoingTransaction>> GetAllOutgoingTransactionsForWalletAccount(string? account, DateTime? startDate)
  {
    var allTransactions = await GetAllTransactionsForWalletAccount(account, startDate);
    return (from transaction in allTransactions
      where transaction.Type == TransactionType.Payment
      select new OutgoingTransaction
      {
        Amount = transaction.Amount,
        Comment = transaction.Comment,
        DestinationAddress = transaction.DestinationAddress,
        SourceAddress = transaction.SourceAddress,
        Timestamp = transaction.Timestamp,
        Type = transaction.Type
      }).ToList();
  }

  private static List<Transaction> FilterTransactions(IEnumerable<Shared.Models.TonApi.Transactions.Transaction> walletTransactions)
  {
    var allTransactions = new List<Transaction>();

    foreach (var walletTransaction in walletTransactions)
    {
      if (walletTransaction.InMsg is not null &&
          walletTransaction.InMsg.Value is not null &&
          walletTransaction.InMsg.MessageContent.Decoded is not null)
      {
        var inMsg = walletTransaction.InMsg;
        var incomingTransaction = new IncomingTransaction()
        {
          Amount = FromNano(decimal.Parse(inMsg.Value))
        };
        var decodedComment = ((JsonElement)inMsg.MessageContent.Decoded).Deserialize<DecodedComment>();
        incomingTransaction.Comment = decodedComment.Comment;
        incomingTransaction.DestinationAddress = inMsg.Destination.Split(':')[1];
        incomingTransaction.SourceAddress = inMsg.Source.Split(':')[1];
        incomingTransaction.Type = TransactionType.Purchase;
        incomingTransaction.Timestamp = DateTimeOffset.FromUnixTimeSeconds(walletTransaction.Now).DateTime;
        allTransactions.Add(incomingTransaction);
      } else if (walletTransaction.OutMsgs is not null &&
                 walletTransaction.OutMsgs.Length > 0)
      {
        var outMsg = walletTransaction.OutMsgs[0];
        if (outMsg.Value is null || 
            outMsg.MessageContent.Decoded is null)
        {
          continue;
        }

        var outgoingTransaction = new OutgoingTransaction()
        {
          Amount = FromNano(decimal.Parse(outMsg.Value))
        };
        var decodedComment = ((JsonElement)outMsg.MessageContent.Decoded).Deserialize<DecodedComment>();
        outgoingTransaction.Comment = decodedComment.Comment;
        outgoingTransaction.DestinationAddress = outMsg.Destination.Split(':')[1];
        outgoingTransaction.SourceAddress = outMsg.Source.Split(':')[1];
        outgoingTransaction.Type = TransactionType.Payment;
        outgoingTransaction.Timestamp = DateTimeOffset.FromUnixTimeSeconds(walletTransaction.Now).DateTime;
        allTransactions.Add(outgoingTransaction);
      }
     
    }
    
    return allTransactions.Where(x => x.Amount > 0).Where(x => x.Comment is not null).ToList();
  }

  public static decimal FromNano(decimal nanoValue)
  {
    return nanoValue / 1000000000;
  }
}

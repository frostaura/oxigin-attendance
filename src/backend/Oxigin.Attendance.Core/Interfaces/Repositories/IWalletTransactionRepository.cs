using Transaction = Oxigin.Attendance.Shared.Models.TonApi.Transactions.Transaction;

namespace Oxigin.Attendance.Core.Interfaces.Repositories;

public interface IWalletTransactionRepository
{
  /// <summary>
  /// Gets the transactions for a wallet account.
  /// </summary>
  /// <param name="account">The wallet account address.</param>
  /// <returns>Returns a list of wallet transactions.</returns>
  Task<IEnumerable<Transaction>> GetTransactionsForWalletAccount(string account);
}

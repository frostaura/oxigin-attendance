using Oxigin.Attendance.Shared.Models.Transactions;

namespace Oxigin.Attendance.Core.Interfaces.Managers
{
  public interface ITransactionsManager
  {
    /// <summary>
    /// Gets the transactions for a wallet account.
    /// </summary>
    /// <param name="account">The wallet account address.</param>
    /// <param name="startDate">The start date of when to get transactions from. Defaults to the last week, starting Monday at midnight UTC.</param>
    /// <returns>Returns a list of wallet transactions.</returns>
    Task<IEnumerable<Transaction>> GetAllTransactionsForWalletAccount(string? account, DateTime? startDate);

    /// <summary>
    /// Gets all incoming transactions for a wallet account.
    /// </summary>
    /// <param name="account">The wallet account address.</param>
    /// <param name="startDate">The start date of when to get transactions from. Defaults to the last week, starting Monday at midnight UTC.</param>
    /// <returns>Returns a list of wallet transactions.</returns>
    Task<IEnumerable<IncomingTransaction>> GetAllIncomingTransactionsForWalletAccount(string? account, DateTime? startDate);

    /// <summary>
    /// Gets all outgoing transactions for a wallet account.
    /// </summary>
    /// <param name="account">The wallet account address.</param>
    /// <param name="startDate">The start date of when to get transactions from. Defaults to the last week, starting Monday at midnight UTC.</param>
    /// <returns>Returns a list of wallet transactions.</returns>
    Task<IEnumerable<OutgoingTransaction>> GetAllOutgoingTransactionsForWalletAccount(string? account, DateTime? startDate);
  }
}

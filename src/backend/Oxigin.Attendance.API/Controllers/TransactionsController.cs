using Oxigin.Attendance.API.Abstractions;
using Oxigin.Attendance.Core.Interfaces.Managers;
using Oxigin.Attendance.Shared.Models.Transactions;
using Microsoft.AspNetCore.Mvc;

namespace Oxigin.Attendance.API.Controllers;

[ApiController]
[Route("[controller]")]
public class TransactionsController : BaseController
{
  /// <summary>
  /// A manager to facilitate retrieving transactions for a wallet.
  /// </summary>
  private readonly ITransactionsManager _transactionsManager;

  /// <summary>
  /// Overloaded constructor to allow for injecting dependencies.
  /// </summary>
  /// <param name="transactionsManager"> A manager to facilitate retrieving transactions for a wallet.</param>
  /// <param name="logger">The controller logger instance.</param>
  public TransactionsController(ITransactionsManager transactionsManager, ILogger<TransactionsController> logger)
    : base(logger)
  {
    _transactionsManager = transactionsManager;
  }

  /// <summary>
  /// Get transactions.
  /// </summary>
  /// <param name="account">The wallet account address.</param>
  /// <param name="startDate">The start date of when to get transactions from. Defaults to the last week, starting Monday at midnight UTC.</param>
  /// <param name="token">Token to cancel downstream operations.</param>
  /// <returns>A collection of active affiliates.</returns>
  [HttpGet(Name = "GetTransactionsAsync")]
  public async Task<IEnumerable<Transaction>> GetAsync(string account, DateTime? startDate, CancellationToken token)
  {
    return await _transactionsManager.GetAllTransactionsForWalletAccount(account, startDate);
  }
}
using Oxigin.Attendance.Shared.Models.Entities;
using Oxigin.Attendance.Shared.Models.SmartContract;
using Oxigin.Attendance.Shared.Models.Transactions;

namespace Oxigin.Attendance.Core.Interfaces.Managers;

/// <summary>
/// A manager to facilitate lottery-related use cases.
/// </summary>
public interface ILotteryManager
{
  /// <summary>
  /// Draw lottery numbers.
  /// </summary>
  /// <param name="token">A token to allow for cancelling downstream operations.</param>
  /// <returns></returns>
  Task DrawAsync(CancellationToken token);

  /// <summary>
  /// Retrieve the current state of the lottery
  /// </summary>
  /// <param name="token">A token to allow for cancelling downstream operations.</param>
  /// <returns>The lottery state containing the config and state objects.</returns>
  Task<CompositeState> GetLotteryStateAsync(CancellationToken token);

  /// <summary>
  /// Retrieve all lottery entries.
  /// </summary>
  /// <param name="numberOfDrawRepeats"></param>
  /// <param name="lengthOfLottery"></param>
  /// <param name="token"></param>
  /// <returns></returns>
  Task<List<LotteryTransaction>> GetAllLotteryEntriesAsync(int numberOfDrawRepeats, int lengthOfLottery, CancellationToken token);

  /// <summary>
  /// Retrieve all valid lottery entries.
  /// </summary>
  /// <param name="lotteryEntries"></param>
  /// <param name="discountFactor"></param>
  /// <param name="token"></param>
  /// <returns></returns>
  Task<List<LotteryEntry>> GetAllValidLotteryEntriesAsync(List<LotteryTransaction> lotteryEntries, DiscountFactor discountFactor, CancellationToken token);

  /// <summary>
  /// Retrieve all winning lottery entries.
  /// </summary>
  /// <param name="validLotteryEntries">A list of all valid lottery entries.</param>
  /// <param name="draws"></param>
  /// <param name="token">A token to allow for cancelling downstream operations.</param>
  /// <returns>A list of all winning lottery entries.</returns>
  Task<List<KeyValuePair<LotteryEntry, int>>> GetAllWinningLotteryEntriesAsync(List<LotteryEntry> validLotteryEntries, CompositeState newState, CancellationToken token);

  /// <summary>
  /// Publish the winners of the lottery.
  /// </summary>
  /// <param name="winningEntries">A list of all winning lottery entries.</param>
  /// <param name="token">A token to allow for cancelling downstream operations.</param>
  /// <returns></returns>
  Task PublishWinnersAsync(List<KeyValuePair<LotteryEntry, int>> winningEntries, CancellationToken token);

  /// <summary>
  /// Deduct the admin fee from the lottery state.
  /// </summary>
  /// <param name="lotteryState">The lottery state containing the config and state objects.</param>
  /// <param name="token">A token to allow for cancelling downstream operations.</param>
  /// <returns></returns>
  Task DeductAdminFeeAsync(CompositeState lotteryState, CancellationToken token);

  /// <summary>
  /// Update the absolute jackpot balance.
  /// </summary>
  /// <param name="state">The current lottery state containing the config and state objects.</param>
  /// <param name="updatedJackpotAmount">The updated jackpot amount</param>
  /// <param name="token">A token to allow for cancelling downstream operations.</param>
  /// <returns></returns>
  Task UpdateJackpotAmountAsync(CompositeState state, int updatedJackpotAmount, CancellationToken token);
}

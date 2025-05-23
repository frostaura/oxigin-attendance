namespace Oxigin.Attendance.Core.Interfaces.Managers;

/// <summary>
/// A manager to facilitate payout-related use cases.
/// </summary>
public interface IPayoutManager
{
  /// <summary>
  /// Initiates the payout process for the winners of the lottery.
  /// </summary>
  /// <param name="paymentAccountId">The ID of the source account for the payout.</param>
  /// <param name="paymentAccountType">The type of the source account for the payout.</param>
  /// <param name="payees">A dictionary containing the blockchain addresses and payout amounts for each winner destination account.</param>
  /// <param name="assetId">The ID of the asset used in the payout.</param>
  /// <param name="token">A token to allow for cancelling downstream operations.</param>
  /// <returns>An identifier associated with the payout transaction.</returns>
  Task<string> PayoutWinnersAsync(string paymentAccountId, string paymentAccountType, Dictionary<string, double> payees, string assetId, CancellationToken token);

  /// <summary>
  /// Initiates the payout process for the affiliates of the lottery.
  /// </summary>
  /// <param name="paymentAccountId">The ID of the source account for the payout.</param>
  /// <param name="paymentAccountType">The type of the source account for the payout.</param>
  /// <param name="payees">A dictionary containing the blockchain addresses and payout amounts for each affiliate destination account.</param>
  /// <param name="assetId">The ID of the asset used in the payout.</param>
  /// <param name="token">A token to allow for cancelling downstream operations.</param>
  /// <returns>An identifier associated with the payout transaction.</returns>
  Task<string> PayoutAffiliatesAsync(string paymentAccountId, string paymentAccountType, Dictionary<string, double> payees, string assetId, CancellationToken token);
}

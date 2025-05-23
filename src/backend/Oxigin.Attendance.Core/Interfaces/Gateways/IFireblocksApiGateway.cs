using Oxigin.Attendance.Shared.Models.FireblocksApi.Transactions;

namespace Oxigin.Attendance.Core.Interfaces.Gateways;

/// <summary>
/// Contract for interacting with the Fireblocks network via HTTP.
/// </summary>
public interface IFireblocksApiGateway
{
  /// <summary>
  /// Get the list of external wallets.
  /// </summary>
  /// <returns>The list of external wallets.</returns>
  Task<List<Wallet>> GetExternalWalletsAsync();

  /// <summary>
  /// Get an external wallet by ID.
  /// </summary>
  /// <param name="walletId">The ID of the wallet.</param>
  /// <returns>The external wallet object.</returns>
  Task<Wallet> GetExternalWalletAsync(string walletId);

  /// <summary>
  /// Create a new external wallet.
  /// </summary>
  /// <param name="name">The name of the wallet.</param>
  /// <param name="customerRefId">(Optional) The customer reference ID.</param>
  /// <param name="idempotencyKey">(Optional) The idempotency key.</param>
  /// <returns>The created external wallet object.</returns>
  Task<Wallet> CreateExternalWalletAsync(string name, string customerRefId, string idempotencyKey);

  //// <summary>
  /// Add an asset to an external wallet.
  /// </summary>
  /// <param name="address">The address of the asset.</param>
  /// <param name="walletId">The ID of the external wallet.</param>
  /// <param name="assetId">The ID of the asset.</param>
  /// <param name="tag">(Optional) For XRP wallets, the destination tag; for EOS/XLM, the memo; for the fiat providers (BLINC by BCB Group), the Bank Transfer Description.</param>
  /// <param name="idempotencyKey">(Optional) The idempotency key.</param>
  /// <returns>The added asset object.</returns>
  Task<Asset> AddAssetToExternalWalletAsync(string address, string tag, string walletId, string assetId, string idempotencyKey);

  /// <summary>
  /// Initiates a payout transaction with a Fireblocks instruction set.
  /// </summary>
  /// <param name="paymentAccount">The payment account to use for the transaction.</param>
  /// <param name="instructionSet">The set of instructions for the transaction.</param>
  /// <returns>The response containing the payout transaction details.</returns>
  Task<PayoutTransactionResponse> InitiatePayoutTransactionAsync(Account paymentAccount,  IEnumerable<InstructionSet> instructionSet);

  /// <summary>
  /// Executes a payout instruction set.
  /// </summary>
  /// <param name="payoutId">The ID of the payout to execute.</param>
  /// <returns>The ID of the executed payout response.</returns>
  Task<string> ExecutePayoutRequestAsync(string payoutId);

  /// <summary>
  /// Retrieves the status of a payout transaction.
  /// </summary>
  /// <param name="payoutId">The ID of the payout transaction to monitor.</param>
  /// <returns>The response containing the payout transaction details.</returns>
  Task<PayoutTransactionResponse> GetPayoutAsync(string payoutId);
}

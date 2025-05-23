using Oxigin.Attendance.Core.Extensions;
using Oxigin.Attendance.Core.Interfaces.Gateways;
using Oxigin.Attendance.Core.Interfaces.Managers;
using Oxigin.Attendance.Datastore.Interfaces;
using Oxigin.Attendance.Shared.Models.FireblocksApi.Transactions;
using Microsoft.Extensions.Logging;

namespace Oxigin.Attendance.Core.Services.Managers
{
  public class PayoutManager : IPayoutManager
  {
    /// <summary>
    /// The datastore context instance.
    /// </summary>
    private readonly IDatastoreContext _datastoreContext;
    /// <summary>
    /// The logger instance.
    /// </summary>
    private readonly ILogger<TransactionsManager> _logger;
    /// <summary>
    /// The Fireblocks API gateway instance.
    /// </summary>
    private readonly IFireblocksApiGateway _fireblocksApiGateway;

    /// <summary>
    /// Overloaded constructor to allow for injecting dependencies.
    /// </summary>
    /// <param name="datastoreContext">The datastore context instance.</param>
    /// <param name="logger">The logger instance.</param>
    /// <param name="fireblocksApiGateway">The Fireblocks API gateway instance.</param>
    public PayoutManager(IDatastoreContext datastoreContext, 
      ILogger<TransactionsManager> logger,
      IFireblocksApiGateway fireblocksApiGateway)
    {
      _datastoreContext = datastoreContext.ThrowIfNull(nameof(datastoreContext));
      _logger = logger;
      _fireblocksApiGateway = fireblocksApiGateway;
    }

    /// <summary>
    /// Initiates the payout process for the winners of the lottery.
    /// </summary>
    /// <param name="paymentAccountId">The ID of the source account for the payout.</param>
    /// <param name="paymentAccountType">The type of the source account for the payout.</param>
    /// <param name="payees">A dictionary containing the blockchain addresses and payout amounts for each winner destination account.</param>
    /// <param name="assetId">The ID of the asset used in the payout.</param>
    /// <param name="token">A token to allow for cancelling downstream operations.</param>
    /// <returns>An identifier associated with the payout transaction.</returns>
    public async Task<string> PayoutWinnersAsync(string paymentAccountId, string paymentAccountType, Dictionary<string, double> payees, string assetId, CancellationToken token)
    {
      var paymentAccount = new Account
      {
        Id = paymentAccountId,
        Type = paymentAccountType
      };
      var instructionSet = new List<InstructionSet> { };

      // Get list of external wallets
      var externalWallets = await _fireblocksApiGateway.GetExternalWalletsAsync();
      Asset asset = new Asset();

      foreach (var blockchainAddress in payees.Keys)
      {
        var walletName = $"Payee: {blockchainAddress}";
        var externalWallet = await GetExternalWallet(externalWallets, walletName);
        var externalWalletAsset = await GetExternalWalletAsset(externalWallet, blockchainAddress);

        await AddPayoutToInstructionSet(instructionSet, externalWallet.Id, "EXTERNAL_WALLET", payees[blockchainAddress].ToString(), assetId);
      }

      var payoutResponse = await _fireblocksApiGateway.InitiatePayoutTransactionAsync(paymentAccount, instructionSet);
      return "Success";
    }

    /// <summary>
    /// Initiates the payout process for the affiliates of the lottery.
    /// </summary>
    /// <param name="paymentAccountId">The ID of the source account for the payout.</param>
    /// <param name="paymentAccountType">The type of the source account for the payout.</param>
    /// <param name="payees">A dictionary containing the blockchain addresses and payout amounts for each affiliate destination account.</param>
    /// <param name="assetId">The ID of the asset used in the payout.</param>
    /// <param name="token">A token to allow for cancelling downstream operations.</param>
    /// <returns>An identifier associated with the payout transaction.</returns>
    public async Task<string> PayoutAffiliatesAsync(string paymentAccountId, string paymentAccountType, Dictionary<string, double> payees, string assetId, CancellationToken token)
    {
      var paymentAccount = new Account
      {
        Id = paymentAccountId,
        Type = paymentAccountType
      };
      var instructionSet = new List<InstructionSet> { };

      // Get list of external wallets
      var externalWallets = await _fireblocksApiGateway.GetExternalWalletsAsync();
      Asset asset = new Asset();

      foreach (var blockchainAddress in payees.Keys)
      {
        var walletName = $"Payee: {blockchainAddress}";
        var externalWallet = await GetExternalWallet(externalWallets, walletName);
        var externalWalletAsset = await GetExternalWalletAsset(externalWallet, blockchainAddress);

        await AddPayoutToInstructionSet(instructionSet, externalWallet.Id, "EXTERNAL_WALLET", payees[blockchainAddress].ToString(), assetId);
      }

      var payoutResponse = await _fireblocksApiGateway.InitiatePayoutTransactionAsync(paymentAccount, instructionSet);
      return "Success";
    }

    /// <summary>
    /// Create a new external wallet, or retrieve an existing one if available.
    /// </summary>
    /// <param name="externalWallets">The list of external wallets.</param>
    /// <param name="walletName">The name of the wallet.</param>
    /// <returns>The external wallet object.</returns>
    private async Task<Wallet> GetExternalWallet(List<Wallet> externalWallets, string walletName)
    {
      Wallet externalWallet = new Wallet();

      // Find the existing wallet object (if any)
      var matchingWallet = externalWallets.FirstOrDefault(wallet => wallet.Name == walletName);

      if (matchingWallet != null)
      {
        // Wallet object with matching name found
        externalWallet = matchingWallet;
      }
      else
      {
        // No wallet object with matching name found
        var idempotency = Guid.NewGuid().ToString();
        var customerReferenceId = idempotency.Replace("-", "_");
        externalWallet = await _fireblocksApiGateway.CreateExternalWalletAsync(walletName, customerReferenceId, idempotency);
      }

      return externalWallet;
    }

    /// <summary>
    /// Create an asset on an external wallet, or retrieve an existing one if available.
    /// </summary>
    /// <param name="externalWallet">The external wallet object.</param>
    /// <param name="assetAddress">The address of the asset.</param>
    /// <returns>The asset object.</returns>
    private async Task<Asset> GetExternalWalletAsset(Wallet externalWallet, string assetAddress)
    {
      Asset asset = new Asset();

      // Check if asset already exists on external wallet
      var matchingAsset = externalWallet.Assets.FirstOrDefault(asset => asset.Address == assetAddress);
      if (matchingAsset != null)
      {
        // Asset object with matching address found
        asset = matchingAsset;
      }
      else
      {
        var idempotency = Guid.NewGuid().ToString();
        // No asset object with matching address found
        asset = await _fireblocksApiGateway.AddAssetToExternalWalletAsync(assetAddress, "", externalWallet.Id, "TON_TEST", idempotency);
      }

      return asset;
    }

    /// <summary>
    /// Add a payout to the instruction set.
    /// </summary>
    /// <param name="instructionSet">The list of instruction sets.</param>
    /// <param name="payeeAccountId">The ID of the payee destination account.</param>
    /// <param name="payeeAccountType">The type of the payee destination account.</param>
    /// <param name="paymentAmount">The amount to be paid.</param>
    /// <param name="paymentAssetId">The ID of the payment asset.</param>
    /// <returns>The updated list of instruction sets.</returns>
    private async Task<List<InstructionSet>> AddPayoutToInstructionSet(List<InstructionSet> instructionSet, string payeeAccountId, string payeeAccountType, string paymentAmount, string paymentAssetId)
    {
      var payeeAccount = new Account
      {
        Id = payeeAccountId,
        Type = payeeAccountType
      };

      var payeeAmount = new TransactionAmount
      {
        Amount = paymentAmount,
        AssetId = paymentAssetId
      };

      var idempotency = Guid.NewGuid().ToString();
      var instruction = new InstructionSet
      {
        Id = idempotency,
        PayeeAccount = payeeAccount,
        Amount = payeeAmount
      };

      instructionSet.Add(instruction);

      return instructionSet;
    }
  }
}

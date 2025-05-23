using System.Text.Json;
using Oxigin.Attendance.Core.Interfaces.Gateways;
using Oxigin.Attendance.Shared.Models.Configs;
using Oxigin.Attendance.Shared.Models.TonApi.Transactions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Transaction = Oxigin.Attendance.Shared.Models.TonApi.Transactions.Transaction;
using Oxigin.Attendance.Shared.Models.SmartContract;
using Oxigin.Attendance.Core.Contracts;
using System.Text;

namespace Oxigin.Attendance.Core.Gateways;

public class TonApiGateway : ApiGatewayBase, ITonApiGateway
{
  private readonly ILogger<ApiGatewayBase> _logger;
  private readonly IHttpClientFactory _clientFactory;
  private readonly TonApiConfig _tonApiConfig;
  private int _offset;
  private const int Limit = 1000;
  private bool _hasMoreTransactions = true;

  private const string TransactionsPath = "/api/v3/transactions";

  /// <inheritdoc />
  public TonApiGateway(
    ILogger<ApiGatewayBase> logger,
    IOptions<ResiliencePolicyConfig> resiliencePolicy,
    IHttpClientFactory clientFactory,
    IOptions<TonApiConfig> tonApiConfig)
    : base(logger, resiliencePolicy)
  {
    _logger = logger;
    _clientFactory = clientFactory;
    _tonApiConfig = tonApiConfig.Value;
  }

  /// <summary>
  /// Retrieves transactions for a specific wallet account starting from a given time.
  /// </summary>
  /// <param name="account">The wallet account from which to retrieve transactions.</param>
  /// <param name="startTime">The starting time for retrieving transactions.</param>
  /// <returns>A collection of transactions for the specified wallet account.</returns>
  public async Task<IEnumerable<Transaction>> GetTransactionsForWalletAccount(string account, long startTime)
  {
    _logger.LogDebug("GetTransactionsForWalletAccount start. WalletId: {WalletId}.", account);
    var allTransactions = new List<Transaction>();
    var client = _clientFactory.CreateClient();

    while (_hasMoreTransactions)
    {
      var url = $"{_tonApiConfig.BaseApiUri}{TransactionsPath}?account={account}&limit={Limit}&offset={_offset}&start_utime={startTime}&sort=asc";
      var request = new HttpRequestMessage(HttpMethod.Get, url);

      var response = await ExecuteResilientRequestAsync(async () => await client.SendAsync(request));
      response.EnsureSuccessStatusCode();
      var getResponseDetails = await response.Content.ReadAsStringAsync();

      var walletTransactions = JsonSerializer.Deserialize<WalletTransactionResponse>(getResponseDetails);
      allTransactions.AddRange(walletTransactions.Transactions);
      if (walletTransactions.Transactions.Count() < Limit)
      {
        _hasMoreTransactions = false;
      }

      _offset += Limit;
    }

    _logger.LogDebug("GetTransactionsForWalletAccount end. WalletId: {WalletId}.", account);

    return allTransactions;
  }
  
  /// <summary>
  /// Get the overall state from the smart contract.
  /// </summary>
  /// <param name="token">A token for cancelling downstream operations.</param>
  /// <returns>The overall state of the smart contract, including the config and state objects.</returns>
  public async Task<CompositeState> GetStateFromContractAsync(CancellationToken token)
  {
    _logger.LogDebug("GetStateFromContractAsync start. Contract Address: {ContractAddress}.", _tonApiConfig.SmartContractAddress);

		// We wait on each task in sync because the toncenter API has 1 req/sec quotas.
		var contractInst = new LotteryState(_clientFactory, _tonApiConfig);
    var contractConfig = await contractInst.GetConfigAsync(token);
		var contractState = await contractInst.GetStateAsync(token);

    return new CompositeState
    {
      Config = contractConfig,
      State = contractState
    };
  }

	/// <summary>
	/// Update the state objects on the smart contract. Typically used for updating balances etc.
	/// </summary>
	/// <param name="updatedState">The updated state object to set onto the smart contract.</param>
	/// <param name="token">A token for cancelling downstream operations.</param>
	public Task SetStateOnContractAsync(State updatedState, CancellationToken token)
  {
    if (updatedState == null) throw new ArgumentNullException($"A valid {nameof(updatedState)} is required.");

    var contractInst = new LotteryState(_clientFactory, _tonApiConfig);

    return contractInst.SetStateAsync(updatedState, token);
  }

	/// <summary>
	/// Call the smart contract in order to initiate a new draw, resulting in generating a new set of numbers based on the predefined rules and finally return those numbers together with the exact time that the draw occured.
	/// </summary>
	/// <param name="token">A token for cancelling downstream operations.</param>
	/// <returns></returns>
	public async Task InitiateDrawOnContractAsync(CancellationToken token)
  {
		_logger.LogDebug("InitiateDrawOnContractAsync start. Contract Address: {ContractAddress}.", _tonApiConfig.SmartContractAddress);

    var contractInst = new LotteryState(_clientFactory, _tonApiConfig);

    await contractInst.DrawAsync(token);
  }
}

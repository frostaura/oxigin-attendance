using System.Text;
using System.Text.Json;
using Oxigin.Attendance.Core.Interfaces.Gateways;
using Oxigin.Attendance.Shared.Models.Configs;
using Oxigin.Attendance.Shared.Models.FireblocksApi.Transactions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Oxigin.Attendance.Core.Services.Managers;

namespace Oxigin.Attendance.Core.Gateways
{
  public class FireblocksApiGateway : ApiGatewayBase, IFireblocksApiGateway
  {
    private readonly ILogger<ApiGatewayBase> _logger;
    private readonly IHttpClientFactory _clientFactory;
    private readonly FireblocksApiConfig _fireblocksApiConfig;
    private const string ExternalWalletsPath = "/v1/external_wallets";
    private const string TransactionsPath = "/v1/payments/payout";

    /// <inheritdoc />
    public FireblocksApiGateway(
      ILogger<ApiGatewayBase> logger,
      IOptions<ResiliencePolicyConfig> resiliencePolicy,
      IHttpClientFactory clientFactory,
      IOptions<FireblocksApiConfig> fireblocksApiConfig)
      : base(logger, resiliencePolicy)
    {
      _logger = logger;
      _clientFactory = clientFactory;
      _fireblocksApiConfig = fireblocksApiConfig.Value;
    }

    /// <summary>
    /// Get the list of external wallets.
    /// </summary>
    /// <returns>The list of external wallets.</returns>
    public async Task<List<Wallet>> GetExternalWalletsAsync()
    {
      _logger.LogDebug("GetExternalWalletsAsync start.");

      var response = await SendApiRequestAsync(HttpMethod.Get, ExternalWalletsPath);
      var getResponseDetails = await response.Content.ReadAsStringAsync();
      var wallets = JsonSerializer.Deserialize<List<Wallet>>(getResponseDetails);

      _logger.LogDebug("GetExternalWalletsAsync end.");
      return wallets;
    }

    /// <summary>
    /// Get an external wallet by ID.
    /// </summary>
    /// <param name="walletId">The ID of the wallet.</param>
    /// <returns>The external wallet object.</returns>
    public async Task<Wallet> GetExternalWalletAsync(string walletId)
    {
      _logger.LogDebug("GetExternalWalletAsync start.");

      var response = await SendApiRequestAsync(HttpMethod.Get, $"{ExternalWalletsPath}/{walletId}");
      var getResponseDetails = await response.Content.ReadAsStringAsync();
      var wallet = JsonSerializer.Deserialize<Wallet>(getResponseDetails);

      _logger.LogDebug("GetExternalWalletAsync end.");
      return wallet;
    }

    /// <summary>
    /// Create a new external wallet.
    /// </summary>
    /// <param name="name">The name of the wallet.</param>
    /// <param name="customerRefId">(Optional) The customer reference ID.</param>
    /// <param name="idempotencyKey">(Optional) The idempotency key.</param>
    /// <returns>The created external wallet object.</returns>
    public async Task<Wallet> CreateExternalWalletAsync(string name, string customerRefId = "", string idempotencyKey = "")
    {
      _logger.LogDebug("CreateExternalWalletAsync start. WalletName: {name}.", name);
      
      var requestBody = new
      {
        name = name,
        customerRefId = customerRefId
      };
      var requestBodyJson = JsonSerializer.Serialize(requestBody);

      var response = await SendApiRequestAsync(HttpMethod.Post, ExternalWalletsPath, requestBodyJson);
      var getResponseDetails = await response.Content.ReadAsStringAsync();
      var wallet = JsonSerializer.Deserialize<Wallet>(getResponseDetails);

      _logger.LogDebug("CreateExternalWalletAsync end. WalletName: {name}.", name);
      return wallet;
    }

    /// <summary>
    /// Add an asset to an external wallet.
    /// </summary>
    /// <param name="address">The address of the asset.</param>
    /// <param name="walletId">The ID of the external wallet.</param>
    /// <param name="assetId">The ID of the asset.</param>
    /// <param name="tag">(Optional) For XRP wallets, the destination tag; for EOS/XLM, the memo; for the fiat providers (BLINC by BCB Group), the Bank Transfer Description.</param>
    /// <param name="idempotencyKey">(Optional) The idempotency key.</param>
    /// <returns>The added asset object.</returns>
    public async Task<Asset> AddAssetToExternalWalletAsync(string address, string walletId, string assetId, string tag = "", string idempotencyKey = "")
    {
      _logger.LogDebug("AddAssetToExternalWalletAsync start. AssetAddress: {address}.", address);

      var requestBody = new
      {
        address = address,
        tag = tag
      };
      var requestBodyJson = JsonSerializer.Serialize(requestBody);

      var response = await SendApiRequestAsync(HttpMethod.Post, $"{ExternalWalletsPath}/{walletId}/{assetId}", requestBodyJson);
      var getResponseDetails = await response.Content.ReadAsStringAsync();
      var asset = JsonSerializer.Deserialize<Asset>(getResponseDetails);

      _logger.LogDebug("AddAssetToExternalWalletAsync end. AssetAddress: {address}.", address);
      return asset;
    }

    /// <summary>
    /// Initiates a payout transaction with a Fireblocks instruction set.
    /// </summary>
    /// <param name="paymentAccount">The payment account to use for the transaction.</param>
    /// <param name="instructionSet">The set of instructions for the transaction.</param>
    /// <returns>The response containing the payout transaction details.</returns>
    public async Task<PayoutTransactionResponse> InitiatePayoutTransactionAsync(Account paymentAccount, IEnumerable<InstructionSet> instructionSet)
    {
      _logger.LogDebug("InitiatePayoutTransactionAsync start. PaymentAccount: {paymentAccountID}.", paymentAccount.Id);
      var requestBody = new
      {
        paymentAccount = paymentAccount,
        instructionSet = instructionSet
      };
      var requestBodyJson = JsonSerializer.Serialize(requestBody);

      var response = await SendApiRequestAsync(HttpMethod.Post, TransactionsPath, requestBodyJson);
      var getResponseDetails = await response.Content.ReadAsStringAsync();
      var payoutTransaction = JsonSerializer.Deserialize<PayoutTransactionResponse>(getResponseDetails);

      _logger.LogDebug("InitiatePayoutTransactionAsync end. PaymentAccount: {paymentAccountID}.", paymentAccount.Id);
      return payoutTransaction;
    }

    /// <summary>
    /// Executes a payout instruction set.
    /// </summary>
    /// <param name="payoutId">The ID of the payout to execute.</param>
    /// <returns>The ID of the executed payout response.</returns>
    public async Task<string> ExecutePayoutRequestAsync(string payoutId)
    {
      _logger.LogDebug("ExecutePayoutRequestAsync start. PayoutId: {payoutId}.", payoutId);

      var response = await SendApiRequestAsync(HttpMethod.Post, $"{TransactionsPath}/{payoutId}/actions/execute", "");
      var getResponseDetails = await response.Content.ReadAsStringAsync();
      var payoutResponseId = JsonSerializer.Deserialize<string>(getResponseDetails);

      _logger.LogDebug("ExecutePayoutRequestAsync end. PayoutId: {payoutId}.", payoutId);
      return payoutResponseId;
    }

    /// <summary>
    /// Retrieves the status of a payout transaction.
    /// </summary>
    /// <param name="payoutId">The ID of the payout transaction to monitor.</param>
    /// <returns>The response containing the payout transaction details.</returns>
    public async Task<PayoutTransactionResponse> GetPayoutAsync(string payoutId)
    {
      _logger.LogDebug("GetPayoutAsync start. PayoutId: {payoutId}.", payoutId);

      var response = await SendApiRequestAsync(HttpMethod.Post, $"{TransactionsPath}/{payoutId}", "");
      var getResponseDetails = await response.Content.ReadAsStringAsync();
      var payoutTransaction = JsonSerializer.Deserialize<PayoutTransactionResponse>(getResponseDetails);

      _logger.LogDebug("GetPayoutAsync end. PayoutId: {payoutId}.", payoutId);
      return payoutTransaction;
    }

    /// <summary>
    /// Send an API request to the Fireblocks API.
    /// </summary>
    /// <param name="method">The HTTP method of the request.</param>
    /// <param name="path">The path of the API endpoint.</param>
    /// <param name="requestBodyJson">The JSON request body.</param>
    /// <returns>The HTTP response message.</returns>
    private async Task<HttpResponseMessage> SendApiRequestAsync(HttpMethod method, string path, string requestBodyJson = "")
    {
      var client = _clientFactory.CreateClient();
      var url = $"{_fireblocksApiConfig.BaseUrl}{path}";
      var request = new HttpRequestMessage(method, url);

      ApiTokenManager provider = new ApiTokenManager(_fireblocksApiConfig.PrivateKey, _fireblocksApiConfig.ApiKey);
      string token = provider.SignJwt(path, requestBodyJson);

      request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
      request.Headers.Add("X-API-Key", _fireblocksApiConfig.ApiKey);

      if (!string.IsNullOrEmpty(requestBodyJson))
      {
        request.Content = new StringContent(requestBodyJson, Encoding.UTF8, "application/json");
      }

      var response = await client.SendAsync(request);
      response.EnsureSuccessStatusCode();

      return response;
    }
  }
}

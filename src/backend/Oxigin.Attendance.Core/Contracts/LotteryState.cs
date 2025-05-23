using Oxigin.Attendance.Shared.Models.Configs;
using System.Text;
using Oxigin.Attendance.Shared.Models.SmartContract;
using TonSdk.Contracts.Wallet;
using TonSdk.Core.Block;
using System.Text.Json;
using TonSdk.Client;
using TonSdk.Core.Crypto;
using TonSdk.Core;
using TonSdk.Core.Boc;
using Oxigin.Attendance.Shared.Extensions.SmartContract;
using System.Text.RegularExpressions;

namespace Oxigin.Attendance.Core.Contracts;

/// <summary>
/// A representation and encaptulation of the LotteryState smart contract and comms with it.
/// </summary>
public class LotteryState : WalletBase
{
	/// <summary>
	/// The HTTP client context provider.
	/// </summary>
	private IHttpClientFactory _httpClientFactory { get; set; }
	/// <summary>
	/// The comms configuration for the TON API(s).
	/// </summary>
	private TonApiConfig _tonApiConfig { get; set; }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="exitCode"></param>
  /// <exception cref="Exception"></exception>
  private void HandleGetterMethodResponse(int exitCode)
  {
    var exitCodeExceptions = new Dictionary<int, string>
    {
        { 2, "Exit code 2: Stack underflow - An operation consumed more elements than there were on the stacks." },
        { 3, "Exit code 3: Stack overflow - Too many elements copied into a closure continuation or stored on the stack." },
        { 4, "Exit code 4: Integer overflow - Value in calculation goes beyond the range from -2^(256) to 2^(256)-1, or there has been an attempt to divide or modulo by zero." },
        { 5, "Exit code 5: Integer out of range - An attempt was made to store an unexpected amount of data or specify an out-of-bounds value." },
        { 6, "Exit code 6: Invalid opcode - An instruction was specified that is not defined in the current TVM version." },
        { 7, "Exit code 7: Type check error - An argument to a primitive is of an incorrect value type or there has been some other mismatch in types during the compute phase." },
        { 8, "Exit code 8: Cell overflow - An attempt was made to store more than 1023 bits of data, or more than 4 references to other Cells, in a single Cell." },
        { 9, "Exit code 9: Cell underflow - A Slice was used to parse a Cell, while attempting to load more data or references than Slice contains. The most common cause is a mismatch between the expected and actual memory layouts or the Cells." },
        { 10, "Exit code 10: Dictionary error - An incorrect manipulation of dictionaries, such as improper assumptions about their memory layout, has occurred." },
        { 11, "Exit code 11: 'Unknown' error - An unknown error occurred. Most commonly associated with problems encountered when queuing a message or using get-methods." },
        { 12, "Exit code 12: Fatal error - Thrown by TVM in situations deemed impossible." },
        { 13, "Exit code 13: Out of gas error - Insufficient gas to end computations in the compute phase." },
        { -14, "Exit code -14: Out of gas error - Insufficient gas to end computations in the compute phase." }
    };

    if (exitCodeExceptions.ContainsKey(exitCode))
    {
      throw new Exception(exitCodeExceptions[exitCode]);
    }
  }

  /// <summary>
  /// An overloaded contructor for passing arguments.
  /// </summary>
  /// <param name="address">The contract address on-chain.</param>
  public LotteryState(IHttpClientFactory httpClientFactory, TonApiConfig config)
		:base()
	{
		_httpClientFactory = httpClientFactory;
		_tonApiConfig = config;
	}

	/// <summary>
	/// Call the smart contract to get and parse it's config state object.
	/// </summary>
	/// <param name="token">A token to cancel downstream operations.</param>
	/// <returns>The parsed config object.</returns>
	/// <exception cref="HttpIOException">When any HTTP error occurs.</exception>
	public async Task<Config> GetConfigAsync(CancellationToken token)
	{
		var client = _httpClientFactory.CreateClient();
		var responseMessage = await client.PostAsync(_tonApiConfig.GetterUrl, new StringContent(JsonSerializer.Serialize(new
		{
			address = _tonApiConfig.SmartContractAddress,
			method = "config",
			stack = new List<object>()
		}), Encoding.UTF8, "application/json"), token);
		var getConfigResponseStr = await responseMessage.Content.ReadAsStringAsync(token);

		if (!responseMessage.IsSuccessStatusCode) throw new HttpIOException(HttpRequestError.ConnectionError, getConfigResponseStr);

		var parsedResponse = JsonSerializer.Deserialize<GetterMethodResponse>(getConfigResponseStr);

    HandleGetterMethodResponse(parsedResponse.ExitCode);

    // Read from the stack in order (very important). We can read the things we don't care about as void (for example owner address).
    var address = parsedResponse.Stack.ReadAsCell();
		var requiredNumbersCount = parsedResponse.Stack.ReadAsInt();
		var maxNumberRange = parsedResponse.Stack.ReadAsInt();
		var maxJackpotNumberRange = parsedResponse.Stack.ReadAsInt();
		var maxSupportedRepeatsPerDraw = parsedResponse.Stack.ReadAsInt();
		var defaultRepeatSelection = parsedResponse.Stack.ReadAsInt();
		var daysPerDraw = parsedResponse.Stack.ReadAsInt();
		var discountFactorForEvery = parsedResponse.Stack.ReadAsInt();
		var discountFactorGet = parsedResponse.Stack.ReadAsInt();

		// We wait on each task in sync because the toncenter API has 1 req/sec quotas.
		await Task.Delay(1500);

		return new Config
		{
			DaysPerDraw = daysPerDraw,
			DefaultRepeatSelection = defaultRepeatSelection,
			MaxJackpotNumberRange = maxJackpotNumberRange,
			MaxNumberRange = maxNumberRange,
			MaxSupportedRepeatsPerDraw = maxSupportedRepeatsPerDraw,
			RequiredNumbersCount = requiredNumbersCount,
			DiscountFactor = new DiscountFactor
			{
				ForEvery = discountFactorForEvery,
				Get = discountFactorGet
			}
		};
	}

	/// <summary>
	/// Call the smart contract to get and parse it's state object.
	/// </summary>
	/// <param name="token">A token to cancel downstream operations.</param>
	/// <returns>The parsed state object.</returns>
	/// <exception cref="HttpIOException">When any HTTP error occurs.</exception>
	public async Task<State> GetStateAsync(CancellationToken token)
	{
		var client = _httpClientFactory.CreateClient();
		var responseMessage = await client.PostAsync(_tonApiConfig.GetterUrl, new StringContent(JsonSerializer.Serialize(new
		{
			address = _tonApiConfig.SmartContractAddress,
			method = "state",
			stack = new List<object>()
		}), Encoding.UTF8, "application/json"), token);
		var getStateResponseStr = await responseMessage.Content.ReadAsStringAsync(token);

		if (!responseMessage.IsSuccessStatusCode) throw new HttpIOException(HttpRequestError.ConnectionError, getStateResponseStr);

		var parsedResponse = JsonSerializer.Deserialize<GetterMethodResponse>(getStateResponseStr);

		HandleGetterMethodResponse(parsedResponse.ExitCode);

    // Read from the stack in order (very important). We can read the things we don't care about as void (for example owner address).
    var jackpotRolloverBalance = parsedResponse.Stack.ReadAsInt();
		var pastRepeatedPurchasesBalance = parsedResponse.Stack.ReadAsInt();
		var jackpotAbsoluteBalance = parsedResponse.Stack.ReadAsInt();
		var latestDrawStr = parsedResponse.Stack.ReadAsString().Replace(",]", "]");
		var latestDraw = JsonSerializer.Deserialize<List<int>>(latestDrawStr);

		// We wait on each task in sync because the toncenter API has 1 req/sec quotas.
		await Task.Delay(1500);

		return new State
		{
			LatestDraw = latestDraw,
			JackpotAbsoluteBalance = jackpotAbsoluteBalance,
			JackpotRolloverBalance = jackpotRolloverBalance,
			PastRepeatedPurchasesBalance = pastRepeatedPurchasesBalance
		};
	}

	/// <summary>
	/// Record a collection of numbers the transaction log of the smart contract.
	/// </summary>
	/// <param name="numbers">The collection of numbers to record on-chain.</param>
	/// <param name="token">A token to cancel downstream operations.</param>
	public async Task RecordDrawOnChain(List<int> numbers, CancellationToken token)
	{
		var (client, gasStationWallet, credentials) = GetGasStationWallet();
		var destination = new Address(_tonApiConfig.SmartContractAddress);
		var amount = new Coins(0.05);
		var body = new CellBuilder()
			.StoreUInt(0, 32)
			.StoreString(JsonSerializer.Serialize(new
			{
				e = "draw",
				n = numbers
			}))
			.Build();
		var seqno = await client.Wallet.GetSeqno(gasStationWallet.Address);
		var message = gasStationWallet.CreateTransferMessage(new[]
		{
				new WalletTransfer
				{
						Message = new InternalMessage(new InternalMessageOptions
						{
								Info = new IntMsgInfo(new IntMsgInfoOptions
								{
										Dest = destination,
										Value = amount,
										Bounce = true
								}),
								Body = body
						}),
						Mode = 1
				}
		}, seqno ?? 0);

		message.Sign(credentials.Keys.PrivateKey);

		await client.SendBoc(message.Cell);
	}

	/// <summary>
	/// Initiate the DrawMessage function on the smart contract which would generate and persist a new timestamp and collection of unique numbers for the draw, to the smart contract's state.
	/// </summary>
	/// <param name="token">A token to cancel downstream operations.</param>
	public async Task DrawAsync(CancellationToken token)
	{
		var (client, gasStationWallet, credentials) = GetGasStationWallet();
		var destination = new Address(_tonApiConfig.SmartContractAddress);
		var amount = new Coins(0.05);
		// The header for DrawMessage (this is obtained from the "tact_LotteryState.ts" file and will change on each new contract deployment.
		var methodHeader = 2576429480;
		var queryId = 0;
		var body = new CellBuilder()
			.StoreUInt(methodHeader, 32)
			.StoreUInt(queryId, 64)
			.Build();
		var seqno = await client.Wallet.GetSeqno(gasStationWallet.Address);
		var message = gasStationWallet.CreateTransferMessage(new[]
		{
				new WalletTransfer
				{
						Message = new InternalMessage(new InternalMessageOptions
						{
								Info = new IntMsgInfo(new IntMsgInfoOptions
								{
										Dest = destination,
										Value = amount,
										Bounce = true
								}),
								Body = body
						}),
						Mode = 1
				}
		}, seqno ?? 0);

		message.Sign(credentials.Keys.PrivateKey);

		await client.SendBoc(message.Cell);
		// Artificially delay to allow for the transaction to distribute across the chain (typically 5-15sec).
		await Task.Delay(30000);
	}

	/// <summary>
	/// Initiate the StateUpdateMessage function on the smart contract which would update the smart contract's state (minus the draws object, for that DrawAsync can be called).
	/// </summary>
	/// <param name="updatedState">The updated state object to set on the smart contract.</param>
	/// <param name="token">A token to cancel downstream operations.</param>
	public async Task SetStateAsync(State updatedState, CancellationToken token)
	{
		if (updatedState == null) throw new ArgumentNullException($"A valid {nameof(updatedState)} is required.");
		if (updatedState.JackpotAbsoluteBalance <= 0) throw new ArgumentNullException($"A valid {nameof(updatedState.JackpotAbsoluteBalance)} is required.");
		if (updatedState.JackpotRolloverBalance <= 0) throw new ArgumentNullException($"A valid {nameof(updatedState.JackpotRolloverBalance)} is required.");
		if (updatedState.PastRepeatedPurchasesBalance <= 0) throw new ArgumentNullException($"A valid {nameof(updatedState.PastRepeatedPurchasesBalance)} is required.");

		var (client, gasStationWallet, credentials) = GetGasStationWallet();
		var destination = new Address(_tonApiConfig.SmartContractAddress);
		var amount = new Coins(0.5);
		// The header for StateUpdateMessage (this is obtained from the "tact_LotteryState.ts" file and will change on each new contract deployment.
		var methodHeader = 1749896102;
		var queryId = 0;
		var body = new CellBuilder()
			.StoreUInt(methodHeader, 32)
			.StoreUInt(queryId, 64)
			.StoreUInt(updatedState.JackpotRolloverBalance, 64)
			.StoreUInt(updatedState.JackpotAbsoluteBalance, 64)
			.StoreUInt(updatedState.PastRepeatedPurchasesBalance, 64)
			.Build();
		var seqno = await client.Wallet.GetSeqno(gasStationWallet.Address);
		var message = gasStationWallet.CreateTransferMessage(new[]
		{
				new WalletTransfer
				{
						Message = new InternalMessage(new InternalMessageOptions
						{
								Info = new IntMsgInfo(new IntMsgInfoOptions
								{
										Dest = destination,
										Value = amount,
										Bounce = true
								}),
								Body = body
						}),
						Mode = 1
				}
		}, seqno ?? 0);

		message.Sign(credentials.Keys.PrivateKey);

		await client.SendBoc(message.Cell);
		// Artificially delay to allow for the transaction to distribute across the chain (typically 5-15sec).
		await Task.Delay(30000);
	}

	protected override StateInit buildStateInit()
	{
		throw new NotImplementedException();
	}

	/// <summary>
	/// Reconstruct the gas station wallet using the backup mnemonic phrases from config.
	/// </summary>
	/// <returns>The ton client to leverage to transact, the reconstucted wallet and credentials/keys to sign messages with.</returns>
	private (TonClient client, WalletV4 wallet, Mnemonic credentials) GetGasStationWallet()
	{
		var tonClient = new TonClient(TonClientType.HTTP_TONCENTERAPIV2, new HttpParameters
		{
			Endpoint = _tonApiConfig.BaseUrl
		});
		var gasStationMnemonic = new Mnemonic(_tonApiConfig.GasStationMnemonicPhrase.ToArray());
		var gasStationWalletOptionsV4 = new WalletV4Options()
		{
			PublicKey = gasStationMnemonic.Keys.PublicKey
		};
		var gasStationWalletV4R2 = new WalletV4(gasStationWalletOptionsV4, 2);

		return (tonClient, gasStationWalletV4R2, gasStationMnemonic);
	}
}

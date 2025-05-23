using Oxigin.Attendance.Shared.Models.SmartContract;
using Oxigin.Attendance.Shared.Models.TonApi.Transactions;

namespace Oxigin.Attendance.Core.Interfaces.Gateways;

/// <summary>
/// Contract for interacting with the Ton network via HTTP.
/// </summary>
public interface ITonApiGateway
{
  /// <summary>
  /// Gets the transactions for a wallet account.
  /// </summary>
  /// <param name="account">The wallet account address.</param>
  /// <param name="startTime">The start time in epoch seconds.</param>
  /// <returns>Returns a list of wallet transactions.</returns>
  Task<IEnumerable<Transaction>> GetTransactionsForWalletAccount(string account, long startTime);
  /// <summary>
  /// Get the overall state from the smart contract.
  /// </summary>
  /// <param name="token">A token for cancelling downstream operations.</param>
  /// <returns>The overall state of the smart contract, including the config and state objects.</returns>
  Task<CompositeState> GetStateFromContractAsync(CancellationToken token);
	/// <summary>
	/// Update the state objects on the smart contract. Typically used for updating balances etc.
	/// </summary>
  /// <param name="updatedState">The updated state object to set onto the smart contract.</param>
	/// <param name="token">A token for cancelling downstream operations.</param>
	Task SetStateOnContractAsync(State updatedState, CancellationToken token);
	/// <summary>
	/// Call the smart contract in order to initiate a new draw, resulting in generating a new set of numbers based on the predefined rules and finally return those numbers together with the exact time that the draw occured.
	/// </summary>
	/// <param name="token">A token for cancelling downstream operations.</param>
	/// <returns>/returns>
	Task InitiateDrawOnContractAsync(CancellationToken token);
}

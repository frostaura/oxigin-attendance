namespace Oxigin.Attendance.Shared.Models.SmartContract;

/// <summary>
/// The overall smart contract state containing the config and state objects.
/// </summary>
public class CompositeState
{
	/// <summary>
	/// The configuration object from the smart contract.
	/// </summary>
	public Config Config { get; set; }
	/// <summary>
	/// The state object from the smart contract.
	/// </summary>
	public State State { get; set; }
}

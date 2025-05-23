namespace Oxigin.Attendance.Shared.Models.SmartContract;

/// <summary>
/// The configuration object from the smart contract.
/// </summary>
public class Config
{
	/// <summary>
	/// The address of the owner of the smart contract.
	/// </summary>
	public string OwnerAddress { get; set; }
	/// <summary>
	/// The required count of numbers that should be picked excluding the jackpot ball.
	/// Example: 5.
	/// </summary>
	public int RequiredNumbersCount { get; set; }
	/// <summary>
	/// The maximum number value that may be picked for the normal number range.
	/// Example: 36.
	/// </summary>
	public int MaxNumberRange { get; set; }
	/// <summary>
	/// The maximum number value that may be picked for the jackpot number range.
	/// Example: 10.
	/// </summary>
	public int MaxJackpotNumberRange { get; set; }
	/// <summary>
	/// The maximum supported repeats per/entry. This allows a user to enter the same numbers over multiple draws.
	/// Example: 24.
	/// </summary>
	public int MaxSupportedRepeatsPerDraw { get; set; }
	/// <summary>
	/// The default number of repeats to pre-select on the UI.
	/// </summary>
	public int DefaultRepeatSelection { get; set; }
	/// <summary>
	/// The count of days per/draw period.
	/// Example 7, (for one week draws).
	/// </summary>
	public int DaysPerDraw { get; set; }
	/// <summary>
	/// The discount factor for when purchasing tickets.
	/// </summary>
	public DiscountFactor DiscountFactor { get; set; }
}


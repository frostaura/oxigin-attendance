namespace Oxigin.Attendance.Shared.Models.SmartContract;

/// <summary>
/// The state object from the smart contract.
/// </summary>
public class State
{
	/// <summary>
	/// This is the amount of TON that rolled over into the current period, from the last one (I.e. when nobody won the jackpot).
	/// </summary>
	public int JackpotRolloverBalance { get; set; }
	/// <summary>
	/// This is the amount of repeated past entries that are relevant to the current draw/run. (This speaks to repeating favorite numbers/future stake).
	/// </summary>
	public int PastRepeatedPurchasesBalance { get; set; }
	/// <summary>
	/// The above (jackpot rollover amount) + the above repeated rollover amount + all entries for the current period.
	/// </summary>
	public int JackpotAbsoluteBalance { get; set; }
	/// <summary>
	/// Numbers for the latest draw.
	/// </summary>
	public List<int> LatestDraw = new List<int>();
}


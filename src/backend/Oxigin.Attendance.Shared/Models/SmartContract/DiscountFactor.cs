namespace Oxigin.Attendance.Shared.Models.SmartContract;

/// <summary>
/// The discount factor for when purchasing tickets.
/// Example: The below example spells out that for each 2 tickets purchased, the players would get 1 for free.
/// </summary>
public class DiscountFactor
{
	/// <summary>
	/// For every N-th entry/ticket/repeat.
	/// Example: 2.
	/// </summary>
	public int ForEvery { get; set; }
	/// <summary>
	/// Get N entries/tickets/repeats for free.
	/// Example: 1.
	/// </summary>
	public int Get { get; set; }
}


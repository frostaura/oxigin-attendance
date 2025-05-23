using System.Text;
using Oxigin.Attendance.Shared.Models.SmartContract;
using TonSdk.Core.Boc;

namespace Oxigin.Attendance.Shared.Extensions.SmartContract;

public static class StackExtensions
{
	/// <summary>
	///	Read from the stack buffer a singluar next value as a cell.
	/// 
	/// See: https://github.com/ton-core/ton-core/blob/main/src/boc/BitReader.ts for more on how to parse info inside of cells, if need be.
	/// </summary>
	/// <param name="stack">The response stack from the contract getter method.</param>
	/// <returns>The parsed cell.</returns>
	public static Cell ReadAsCell(this Queue<StackItem> stack)
	{
		var currentItem = stack.Dequeue();
		var cell = new Cell(currentItem.Value);

		return cell;
	}

	/// <summary>
	///	Read from the stack buffer a singluar next value as an int.
	/// </summary>
	/// <param name="stack">The response stack from the contract getter method.</param>
	/// <returns>The parsed int value.</returns>
	public static int ReadAsInt(this Queue<StackItem> stack)
	{
		var currentItem = stack.Dequeue();
		var value = currentItem.Value;

		if (value.StartsWith("0x"))
		{
			// Remove the "0x" prefix.
			value = value.Substring(2);
		}

		var intValue = Convert.ToInt64(value, 16);

		return Convert.ToInt32(intValue);
	}


	/// <summary>
	///	Read from the stack buffer a singluar next value as a string.
	/// </summary>
	/// <param name="stack">The response stack from the contract getter method.</param>
	/// <returns>The parsed string value.</returns>
	public static string ReadAsString(this Queue<StackItem> stack)
	{
		var currentItem = stack.Dequeue();
		var value = currentItem.Value;
		var cell = Cell.From(value);
		var cellSlice = cell.Parse();
		var responseValue = cellSlice.LoadString();

		return responseValue;
	}
}

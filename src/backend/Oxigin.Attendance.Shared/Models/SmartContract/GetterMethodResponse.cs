using System.Diagnostics;
using System.Text.Json.Serialization;

namespace Oxigin.Attendance.Shared.Models.SmartContract;

[DebuggerDisplay("Exit Code: {ExitCode}")]
public class GetterMethodResponse
{
	[JsonPropertyName("exit_code")]
	public int ExitCode { get; set; }

	[JsonPropertyName("gas_used")]
	public int GasUsed { get; set; }

	[JsonPropertyName("stack")]
	public Queue<StackItem> Stack { get; set; }
}

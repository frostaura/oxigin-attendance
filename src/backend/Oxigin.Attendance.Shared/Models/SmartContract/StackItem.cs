using System.Diagnostics;
using System.Text.Json.Serialization;

namespace Oxigin.Attendance.Shared.Models.SmartContract;

[DebuggerDisplay("{Type} => {Value}")]
public class StackItem
{
	[JsonPropertyName("type")]
	public string Type { get; set; }

	[JsonPropertyName("value")]
	public string Value { get; set; }
}

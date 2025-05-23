using Oxigin.Attendance.Shared.Models.Abstractions;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace Oxigin.Attendance.Shared.Models.Entities
{
  /// <summary>
  /// LotteryEntry entity model.
  /// </summary>
  [Table("LotteryEntries")]
  [DebuggerDisplay("Name: {Name}")]
  public class LotteryEntry : BaseNamedEntity
  {
    [JsonPropertyName("n")]
    public IEnumerable<int> Numbers { get; set; }

    [JsonPropertyName("r")]
    public int EntryRepeatCount { get; set; }
  }
}

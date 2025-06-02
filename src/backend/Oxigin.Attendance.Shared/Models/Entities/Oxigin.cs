using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oxigin.Attendance.Shared.Models.Abstractions;

namespace Oxigin.Attendance.Shared.Models.Entities;

/// <summary>
/// Represents the different workers in Oxigin
/// </summary>
[Table("Oxigin")]
public class Oxigin : BaseEntity
{
    /// <summary>
    /// Oxigin Department
    /// </summary>
    [Required(AllowEmptyStrings = false, ErrorMessage = $"Oxigin department required.")]
    public string Department { get; set; }
}
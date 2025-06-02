using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oxigin.Attendance.Shared.Models.Abstractions;

namespace Oxigin.Attendance.Shared.Models.Entities;

/// <summary>
/// Represents clients of Oxigin
/// </summary>
[Table("Clients")]
public class Client : BaseEntity
{
    /// <summary>
    /// Company name
    /// </summary>
    public string CompanyName{ get; set; }
    /// <summary>
    /// Company Registration Number
    /// </summary>
    public string RegNo { get; set; }
    /// <summary>
    /// Company address
    /// </summary>
    public string Address { get; set; }
    /// <summary>
    /// Company contact number
    /// </summary>
    [Required(AllowEmptyStrings = false, ErrorMessage = $"Contact number is required.")]
    public string ContactNo { get; set; }
   }

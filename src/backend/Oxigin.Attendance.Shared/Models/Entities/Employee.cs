using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oxigin.Attendance.Shared.Models.Abstractions;

namespace Oxigin.Attendance.Shared.Models.Entities;

/// <summary>
/// Represents a job request or assignment in the system.
/// </summary>
[Table("Employees")]
public class Employee : BaseEntity
{
    /// <summary>
    /// Employee ID Number
    /// </summary>
    public string IDNumber { get; set; }
    /// <summary>
    /// Employee address
    /// </summary>
    public string Address { get; set; }
    /// <summary>
    /// Contact number for employee
    /// </summary>
    [Required(AllowEmptyStrings = false, ErrorMessage = $"Contact number is required.")]
    public string ContactNo { get; set; }
    /// <summary>
    /// Bank account details to pay employee
    /// </summary>
    public string BankName { get; set; }
    /// <summary>
    /// Bank account details to pay employee
    /// </summary>
    public string AccountHolderName { get; set; }
    /// <summary>
    /// Bank account details to pay employee
    /// </summary>
    public string BranchCode { get; set; }
    /// <summary>
    /// Bank account details to pay employee
    /// </summary>
    public string AccountNumber { get; set; }
    
}

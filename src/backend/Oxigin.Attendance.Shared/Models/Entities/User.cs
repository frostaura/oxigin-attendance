using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Oxigin.Attendance.Shared.Enums;
using Oxigin.Attendance.Shared.Models.Abstractions;

namespace Oxigin.Attendance.Shared.Models.Entities;

/// <summary>
/// An entity representing the users table in the database.
/// </summary>
[Table("Users")]
[Index(nameof(Email), IsUnique = true)]
public class User : BaseNamedEntity
{
    /// <summary>
    /// The unique contact number of the user.
    /// </summary>
    [Required(AllowEmptyStrings = false, ErrorMessage = $"A valid contact nr is required.")]
    public string ContactNr { get; set; }

    /// <summary>
    /// The unique email for the user.
    /// </summary>
    [Required(AllowEmptyStrings = false, ErrorMessage = $"A valid email is required.")]
    public string Email { get; set; }

    /// <summary>
    /// The kind of user. This allows for giving group permissions and such.
    /// </summary>
    [Required]
    public UserType UserType { get; set; }

    /// <summary>
    /// User password, typically hashed.
    /// </summary>
    [Required(AllowEmptyStrings = false, ErrorMessage = $"A valid password is required.")]
    public string Password { get; set; }

    /// <summary>
    /// A collection of the user's sessions.
    /// </summary>
    [JsonIgnore]
    public virtual ICollection<UserSession> Sessions { get; set; } = new List<UserSession>();

    /// <summary>
    /// Foreign key to the Clients table.
    /// Nullable. Renamed to ClientID.
    /// </summary>
    public Guid? ClientID { get; set; }

    /// <summary>
    /// Navigation property for the related Client.
    /// </summary>
    [ForeignKey(nameof(ClientID))]
    public virtual Client? Client { get; set; }

    /// <summary>
    /// Foreign key to the Employees table.
    /// Nullable. Renamed to EmployeeID.
    /// </summary>
    public Guid? EmployeeID { get; set; }

    /// <summary>
    /// Navigation property for the related Employee.
    /// </summary>
    [ForeignKey(nameof(EmployeeID))]
    public virtual Employee? Employee { get; set; }

    /// <summary>
    /// Optional department name.
    /// </summary>
    public string? Department { get; set; }
}

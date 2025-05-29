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
}
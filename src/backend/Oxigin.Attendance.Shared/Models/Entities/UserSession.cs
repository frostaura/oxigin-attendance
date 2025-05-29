using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oxigin.Attendance.Shared.Models.Abstractions;

namespace Oxigin.Attendance.Shared.Models.Entities;

/// <summary>
/// An entity representing a session for a user.
/// </summary>
public class UserSession : BaseEntity
{
    /// <summary>
    /// The unique id of the user associated with this session.
    /// </summary>
    [Required(ErrorMessage = $"A valid user id is required.")]
    public int UserId { get; set; }
    /// <summary>
    /// The context for the user that is mapped to this session.
    /// </summary>
    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; }
}
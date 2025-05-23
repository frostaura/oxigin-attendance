using Oxigin.Attendance.Shared.Models.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Oxigin.Attendance.Shared.Models.Entities
{
    /// <summary>
    /// Affiliate entity model.
    /// </summary>
    [Table("Affiliates")]
    [DebuggerDisplay("Name: {Name}")]
    public class Affiliate : BaseNamedEntity
    {
        /// <summary>
        /// The unique id of the respective employee that the image generation request belongs to.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = $"A valid wallet address is required.")]
        public string WalletAddress { get; set; }
    }
}

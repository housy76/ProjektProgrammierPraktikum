using System.ComponentModel.DataAnnotations;

namespace TerminUndRaumplanung.Models.AccountViewModels
{
    /// <summary>
    /// Model for external Login View
    /// </summary>
    public class ExternalLoginViewModel
    {
        /// <summary>
        /// Email Entity
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}

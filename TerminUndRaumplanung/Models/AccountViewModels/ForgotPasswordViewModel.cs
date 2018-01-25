using System.ComponentModel.DataAnnotations;

namespace TerminUndRaumplanung.Models.AccountViewModels
{
    /// <summary>
    /// Model for forgotten Password View
    /// </summary>
    public class ForgotPasswordViewModel
    {
        /// <summary>
        /// Email Entity
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}

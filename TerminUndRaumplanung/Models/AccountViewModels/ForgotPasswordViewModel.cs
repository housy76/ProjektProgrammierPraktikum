using System.ComponentModel.DataAnnotations;

namespace TerminUndRaumplanung.Models.AccountViewModels
{
    /// <summary>
    /// Model for forgotten Password
    /// </summary>
    public class ForgotPasswordViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}

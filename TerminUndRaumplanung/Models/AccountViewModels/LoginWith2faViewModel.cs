using System.ComponentModel.DataAnnotations;

// Bearbeitet durch Simon Hummel

namespace TerminUndRaumplanung.Models.AccountViewModels
{
    /// <summary>
    /// Model for View for 2-Factor-Authentication
    /// </summary>
    public class LoginWith2faViewModel
    {
        /// <summary>
        /// Display 2FA Code
        /// </summary>
        [Required]
        [StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Authenticator code")]
        public string TwoFactorCode { get; set; }


        /// <summary>
        /// Display Remember Machine
        /// </summary>
        [Display(Name = "Remember this machine")]
        public bool RememberMachine { get; set; }


        /// <summary>
        /// Display Remember Me
        /// </summary>
        public bool RememberMe { get; set; }
    }
}

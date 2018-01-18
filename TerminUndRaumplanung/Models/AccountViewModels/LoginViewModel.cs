using System.ComponentModel.DataAnnotations;

namespace TerminUndRaumplanung.Models.AccountViewModels
{
    /// <summary>
    /// Model for Login View
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// Email Entity
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }


        /// <summary>
        /// Password Entity
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        /// <summary>
        /// Remember me Entity
        /// </summary>
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}

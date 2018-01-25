using System.ComponentModel.DataAnnotations;

namespace TerminUndRaumplanung.Models.AccountViewModels
{
    /// <summary>
    /// Model for Register View
    /// </summary>
    public class RegisterViewModel
    {
        /// <summary>
        /// First Name Entity
        /// </summary>
        [Required]
        [Display(Name = "First Name")]
        public string FirsName { get; set; }


        /// <summary>
        /// Last Name Entity
        /// </summary>
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }


        /// <summary>
        /// Email Entity
        /// </summary>
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}

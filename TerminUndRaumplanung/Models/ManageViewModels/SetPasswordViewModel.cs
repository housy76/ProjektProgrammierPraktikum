using System.ComponentModel.DataAnnotations;

namespace TerminUndRaumplanung.Models.ManageViewModels
{
    /// <summary>
    /// Model for Set Password View
    /// </summary>
    public class SetPasswordViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string StatusMessage { get; set; }
    }
}

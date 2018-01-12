using System.ComponentModel.DataAnnotations;

namespace TerminUndRaumplanung.Models.AccountViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class LoginWith2faViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Authenticator code")]
        public string TwoFactorCode { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "Remember this machine")]
        public bool RememberMachine { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public bool RememberMe { get; set; }
    }
}

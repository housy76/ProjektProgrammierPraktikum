using System.ComponentModel.DataAnnotations;

namespace TerminUndRaumplanung.Models.AccountViewModels
{
    /// <summary>
    /// Model for Login with Recovery Code
    /// </summary>
    public class LoginWithRecoveryCodeViewModel
    {
        /// <summary>
        /// RecoveryCode Entity
        /// </summary>
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Recovery Code")]
            public string RecoveryCode { get; set; }
    }
}

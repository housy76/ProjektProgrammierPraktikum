using System.ComponentModel.DataAnnotations;

namespace TerminUndRaumplanung.Models.AccountViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class LoginWithRecoveryCodeViewModel
    {
        /// <summary>
        /// 
        /// </summary>
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Recovery Code")]
            public string RecoveryCode { get; set; }
    }
}

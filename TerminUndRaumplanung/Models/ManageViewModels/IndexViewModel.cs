using System.ComponentModel.DataAnnotations;

namespace TerminUndRaumplanung.Models.ManageViewModels
{
    /// <summary>
    /// Model for Index View
    /// </summary>
    public class IndexViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string Username { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public bool IsEmailConfirmed { get; set; }


        /// <summary>
        /// Email Entity
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }


        /// <summary>
        ///PphoneNumber Entity
        /// </summary>
        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string StatusMessage { get; set; }
    }
}

﻿using System.ComponentModel.DataAnnotations;

namespace TerminUndRaumplanung.Models.ManageViewModels
{
    /// <summary>
    /// Model for Change Password View
    /// </summary>
    public class ChangePasswordViewModel
    {
        /// <summary>
        /// Old Password Entity
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }


        /// <summary>
        /// New Password Entity
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }


        /// <summary>
        /// Confirm Password Entity
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

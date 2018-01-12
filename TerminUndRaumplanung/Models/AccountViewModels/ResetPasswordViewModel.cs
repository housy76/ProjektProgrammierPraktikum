﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TerminUndRaumplanung.Models.AccountViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class ResetPasswordViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string Code { get; set; }
    }
}

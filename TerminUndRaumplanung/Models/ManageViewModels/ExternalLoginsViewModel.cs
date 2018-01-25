using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace TerminUndRaumplanung.Models.ManageViewModels
{
    /// <summary>
    /// Model for External Login View
    /// </summary>
    public class ExternalLoginsViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public IList<UserLoginInfo> CurrentLogins { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public IList<AuthenticationScheme> OtherLogins { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public bool ShowRemoveButton { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string StatusMessage { get; set; }
    }
}

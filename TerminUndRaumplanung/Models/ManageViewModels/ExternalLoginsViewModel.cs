using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace TerminUndRaumplanung.Models.ManageViewModels
{
    /// <summary>
    /// 
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

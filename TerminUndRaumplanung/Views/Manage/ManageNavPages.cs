using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace TerminUndRaumplanung.Views.Manage
{
    /// <summary>
    /// 
    /// </summary>
    public static class ManageNavPages
    {
        /// <summary>
        /// 
        /// </summary>
        public static string ActivePageKey => "ActivePage";


        /// <summary>
        /// 
        /// </summary>
        public static string Index => "Index";
        
        
        /// <summary>
        /// 
        /// </summary>
        public static string ChangePassword => "ChangePassword";


        /// <summary>
        /// 
        /// </summary>
        public static string ExternalLogins => "ExternalLogins";

        /// <summary>
        /// 
        /// </summary>
        public static string TwoFactorAuthentication => "TwoFactorAuthentication";


        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewContext"></param>
        /// <returns></returns>
        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewContext"></param>
        /// <returns></returns>
        public static string ChangePasswordNavClass(ViewContext viewContext) => PageNavClass(viewContext, ChangePassword);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewContext"></param>
        /// <returns></returns>
        public static string ExternalLoginsNavClass(ViewContext viewContext) => PageNavClass(viewContext, ExternalLogins);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewContext"></param>
        /// <returns></returns>
        public static string TwoFactorAuthenticationNavClass(ViewContext viewContext) => PageNavClass(viewContext, TwoFactorAuthentication);
    

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewContext"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string;
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewData"></param>
        /// <param name="activePage"></param>
        public static void AddActivePage(this ViewDataDictionary viewData, string activePage) => viewData[ActivePageKey] = activePage;
    }
}

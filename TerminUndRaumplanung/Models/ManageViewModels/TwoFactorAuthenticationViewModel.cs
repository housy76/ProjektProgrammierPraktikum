namespace TerminUndRaumplanung.Models.ManageViewModels
{
    /// <summary>
    /// Model for two Factor Authentication View
    /// </summary>
    public class TwoFactorAuthenticationViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public bool HasAuthenticator { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public int RecoveryCodesLeft { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public bool Is2faEnabled { get; set; }
    }
}

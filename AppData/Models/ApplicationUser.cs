using Microsoft.AspNetCore.Identity;


namespace AppData.Models
{
    /// <summary>
    /// Data Model for User
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}

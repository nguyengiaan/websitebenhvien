using Microsoft.AspNetCore.Identity;

namespace websitebenhvien.Models.Enitity
{
    public class ApplicationUser : IdentityUser
    {

        public string FullName { get; set; }

        
    }
}

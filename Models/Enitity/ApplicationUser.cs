using Microsoft.AspNetCore.Identity;

namespace websitebenhvien.Models.Enitity
{
    public class ApplicationUser : IdentityUser
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
    }
}

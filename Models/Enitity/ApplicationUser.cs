using Microsoft.AspNetCore.Identity;

namespace websitebenhvien.Models.Enitity
{
    public class ApplicationUser : IdentityUser
    {

        public ApplicationUser() 
        {
            PermissionUsers = new List<PermissionUser>();
        }

        public string FullName { get; set; }

        public List<PermissionUser> PermissionUsers { get; set; }


    }
}

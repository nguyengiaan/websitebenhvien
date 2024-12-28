namespace websitebenhvien.Models.Enitity
{
    public class PermissionUser
    {
       
        public int id_PermissionUser { get; set; }

        public int id_Permission { get; set; }

        public string id_user { get; set; }

        public Permissions Permissions { get; set; }

        public ApplicationUser User { get; set; }

    }
}



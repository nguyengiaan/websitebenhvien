namespace websitebenhvien.Models.Enitity
{
    public class Permissions
    {
        public Permissions()
        {
            Users = new List<PermissionUser>();
        }
       public int Id { get; set; }

        public string Title_permision { get; set; }

        public List<PermissionUser> Users { get; set; }


        
    }
}

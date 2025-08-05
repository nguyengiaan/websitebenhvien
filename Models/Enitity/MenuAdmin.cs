using System.Reflection.Metadata;

namespace websitebenhvien.Models.Enitity
{
    public class MenuAdmin
    {
        public MenuAdmin()
        {
            MenuAdminUsers = new List<MenuAdminUser>();
        }
        public int id { get; set; }

        public string Title { get; set; }


        public string? Icon { get; set; }


        public string? Url { get; set; }


        public List<MenuAdminUser> MenuAdminUsers { get; set; }



     



    }
}

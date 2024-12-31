using websitebenhvien.Models.Enitity;

namespace websitebenhvien.Models.EnitityVM
{
    public class PemissionUserVM
    {
        public int Id { get; set; }
        public string Id_user { get; set; }

        public string Fullname { get; set; }

        public List<PermissionUser> ds { get; set; } 


    }
}

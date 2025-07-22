using AutoMapper;
using websitebenhvien.Models.Enitity;
using websitebenhvien.Models.EnitityVM;


namespace websitebenhvien.Helper
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            // Ví dụ: map từ MenuAdmin sang MenuAdminDto
            CreateMap<MenuAdmin, MenuAdminVM>().ReverseMap();
        }
    }
}

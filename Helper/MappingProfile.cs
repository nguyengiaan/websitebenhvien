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

            CreateMap<Activitycategory, ActivitycategoryVM>().ReverseMap();

            // Map Postactivity với CategoryName từ navigation property
            CreateMap<Postactivity, PostactivityVM>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Activitycategory != null ? src.Activitycategory.Title : ""))
                .ReverseMap()
                .ForMember(dest => dest.Activitycategory, opt => opt.Ignore());
        }
    }
}

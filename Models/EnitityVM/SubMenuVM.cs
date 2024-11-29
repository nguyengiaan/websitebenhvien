using System.ComponentModel.DataAnnotations;

namespace websitebenhvien.Models.EnitityVM
{
    public class SubMenuVM
    {
        public string ?Id_MenuChild { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tiêu đề")]

        public string Title_MenuChild { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập đường dẫn")]

        public string Link_MenuChild { get; set; }

        public string? Id_menu { get; set; }

        public int Order_menu { get; set; }

        public bool? Status { get; set; }
    }
}

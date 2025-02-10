using System.ComponentModel.DataAnnotations;

namespace websitebenhvien.Models.EnitityVM
{
    public class MachineVM
    {
        public int Id_machine { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên máy")]

        public string Name_machine { get; set; }

        public string Image_machine { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mô tả máy")]    

        public string Description_machine { get; set; }

        public Boolean Status { get; set; }

        public DateTime CreatedBy { get; set; }

        public IFormFile formFile { get; set; }
    }
}

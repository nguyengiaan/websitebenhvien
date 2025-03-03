using System.ComponentModel.DataAnnotations;

namespace websitebenhvien.Models.EnitityVM
{
    public class SpecialtyVM
    {
        public int ?Id_Specialty { get; set; }
        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Giới thiệu không được để trống")]

        public string Introduction { get; set; }
        [Required(ErrorMessage = "Thiết bị không được để trống")]

        public string Machine { get; set; }

        [Required(ErrorMessage = "Dịch vụ không được để trống")]

        public string Service { get; set; }


        public string ?Thumnail { get; set; }
        [Required(ErrorMessage = "Đường dẫn không được để trống")]

        public string Alias_url { get; set; }


        public IFormFile ?formFile { get; set; }
    }
}

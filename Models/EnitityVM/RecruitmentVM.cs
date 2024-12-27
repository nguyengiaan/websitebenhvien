using System;
using System.ComponentModel.DataAnnotations;

namespace websitebenhvien.Models.EnitityVM
{
    public class RecruitmentVM
    {
        public string ?Id_Recruitment { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập thư giới thiệu")]
        public string ?Note { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập vị trí ứng tuyển")]
        public string Position { get; set; }    
        [Required(ErrorMessage = "Vui lòng chọn giới tính")]
        public string Sex { get; set; }
        public DateTime ?CreatedAt { get; set; }
        [Required(ErrorMessage = "Vui lòng tải lên CV")]
        public IFormFile CV_Url { get; set; }
        public string ?CV { get; set; }

    }
}

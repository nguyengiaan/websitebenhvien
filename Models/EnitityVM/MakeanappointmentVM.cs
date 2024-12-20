using System.ComponentModel.DataAnnotations;

namespace websitebenhvien.Models.EnitityVM
{
    public class MakeanappointmentVM
    {
        [Required(ErrorMessage = "Vui lòng nhập tên bác sĩ")]
        public string Name_doctor { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn thời gian khám bệnh")]
        public DateTime Examinationtime { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn tên")]

        public string name { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]

        public string phone { get; set; }
 

        public string? note { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn chuyên khoa")]
        public int Id_Specialty { get; set; }

        public string? Title_Specialty { get; set; }

    }
}

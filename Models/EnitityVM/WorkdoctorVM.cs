using System.ComponentModel.DataAnnotations;

namespace websitebenhvien.Models.EnitityVM
{
    public class WorkdoctorVM
    {
        public int ? Id_workdoctor { get; set; }
        [Required(ErrorMessage = "Ngày tạo không được để trống")]
        public DateTime CreateAt { get; set; }
        [Required(ErrorMessage = "Ngày kết thúc không được để trống")]
        public DateTime EndDate { get; set; }
        public int Id_doctor { get; set; }
        [Required(ErrorMessage = "Lịch làm việc không được để trống")]
        public List<WorkscheduleVM> Workschedules { get; set; }
        public string ? Note { get; set; }
    }
}

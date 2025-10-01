namespace websitebenhvien.Models.EnitityVM
{
    // ViewModel cho mối quan hệ giữa bác sĩ và chuyên khoa
    public class DoctorSpeciallyVM
    {
        public int Id_Specialty { get; set; }
        public string Name_Specialty { get; set; }

        public string Thumnail { get; set; }

        public List<DoctorVM> Doctors { get; set; } = new List<DoctorVM>();
    }
}
namespace websitebenhvien.Models.Enitity
{
    public class Makeanappointment
    {
        public int Id_Make { get; set; }

        public string Name_doctor { get; set; }
        public DateTime Examinationtime { get; set; }

        public string name { get; set; }

        public string phone { get; set; }

        public string ?note { get; set; }
        public int Id_Specialty { get; set; }

        public bool ? Status { get; set; }

        public Specialty Specialty { get; set; }
    }
}

namespace websitebenhvien.Models.Enitity
{
    public class Worksdoctor
    {
        public Worksdoctor()
        {
            Workschedules = new List<Workschedule>();
        }
        public int Id_worksdoctor { get; set; }
        public DateTime CreateAt { get; set; }

        public DateTime EndDate { get; set; }
        public int Id_doctor { get; set; }

        public string ? Note { get; set; }

        public Doctor Doctor { get; set; }

        public List<Workschedule> Workschedules { get; set; }
    }
}

namespace websitebenhvien.Models.EnitityVM
{
    public class WorkscheduleVM
    {
        public int Id_workschedule { get; set; }
        public DateTime Date { get; set; }
        public string Morning { get; set; }
        public string Afternoon { get; set; }
        public string Evening { get; set; }
        public int Id_doctor { get; set; }
    }
}

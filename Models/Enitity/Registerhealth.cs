namespace websitebenhvien.Models.Enitity
{
    public class Registerhealth
    {
        public int Id_Registerhealth { get; set; }

        public DateTime Examinationtime { get; set; }

        public string name { get; set; }

        public string phone { get; set; }

        public string? note { get; set; }

        public bool? Status { get; set; }
    }
}

namespace websitebenhvien.Models.Enitity
{
    public class Workschedule
    {
    
        
        public int Id_workschedule { get; set; }

        public DateTime date { get; set; }

        public string Morning { get; set; }

        public string Afternoon { get; set; }

        public string Evening { get; set; }

        public int Id_worksdoctor { get; set; }

        public Worksdoctor Worksdoctor { get; set; }


    }
} 

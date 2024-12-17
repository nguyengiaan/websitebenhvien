namespace websitebenhvien.Models.Enitity
{
    public class Doctor
    {
        public Doctor() 
        {
            Feeldoctor = new List<Feeldoctor>();
        }
        public int Id_doctor { get; set; }

        public string Name { get; set; }

        public string Thumnail { get; set; }

        public string Introduction { get; set; }


        public string Organization { get; set; }

        public string Award { get; set; }

        public string Research { get; set; }

        public string Training { get; set; }

        public string Experiencework { get; set; }

        public int Specialize { get; set; }

        public Specialty Specialty { get; set; }

        public int Id_specialty { get; set; }

        public List<Feeldoctor> Feeldoctor { get; set; }













    }
}

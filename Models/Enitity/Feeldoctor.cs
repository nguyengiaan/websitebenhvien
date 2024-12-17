namespace websitebenhvien.Models.Enitity
{
    public class Feeldoctor
    {
        public int Id_Feeldoctor { get; set; }


        public string Name { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public int Evaluate { get; set; }


        public string Comment { get; set; }


        public int Id_Doctor { get; set; }

        public Doctor Doctor { get; set; }


    }
}

namespace websitebenhvien.Models.Enitity
{
    public class Menunav
    {
        public int Id_menunav { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        public int Id_titlemenu { get; set; }


        public Titlemenu Titlemenu { get; set; }




    }
}

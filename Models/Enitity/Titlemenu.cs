namespace websitebenhvien.Models.Enitity
{
    public class Titlemenu
    {

        public Titlemenu()
        {
            TitlemenuList = new List<Menunav>();
        }
        public int Id_titlemenu { get; set; }

        public string Name { get; set; }   


        public string Url_icon { get; set; }


        public List<Menunav> TitlemenuList { get; set; } 









    }
}

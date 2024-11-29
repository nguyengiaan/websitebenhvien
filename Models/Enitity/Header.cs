namespace websitebenhvien.Models.Enitity
{
    public class Header
    {
        public Header()
        {
            titleheader = new List<Titleheader>();  
        }
        public string Id_header { get; set; }
        public string logo { get; set; }
        public string telephone { get; set; }
        public List<Titleheader> titleheader { get; set; }  
        
    }
}

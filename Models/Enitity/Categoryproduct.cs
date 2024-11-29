namespace websitebenhvien.Models.Enitity
{
    public class Categoryproduct
    {
        public Categoryproduct()
        {
            Products = new List<Product>();
        }
        public string Id_Categoryproduct { get; set; }
        public string Title { get; set; }
        public string Alias_url { get; set; }

        public string url { get; set; }
        public string ?Description { get; set; }
        public DateTime Createat { get; set; }

        public List<Product> Products { get; set; } 

    }
}

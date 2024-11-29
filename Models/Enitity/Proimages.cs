namespace websitebenhvien.Models.Enitity
{
    public class Proimages
    {
        public string Id_proimages { get; set; }
        public string url { get; set; }
        public string Id_product { get; set; }
        public Product Product { get; set; }
    }
}

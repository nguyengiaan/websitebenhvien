namespace websitebenhvien.Models.Enitity
{
    public class Product
    {
        public Product()
        {
            Proimages = new List<Proimages>();
        }   
        public string Id_product { get; set; }

        public string Title { get; set; }

        public string Alias_url { get; set; }

        public string url { get; set; }

        public string Description { get; set; }

        public DateTime Createat { get; set; }

        public bool status { get; set; }

        public string Id_Categoryproduct { get; set; }

        public string Product_Id { get; set; }

        public Categoryproduct Categoryproduct { get; set; }

        public List<Proimages> Proimages { get; set; }  



    }
}

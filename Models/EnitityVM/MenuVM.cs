namespace websitebenhvien.Models.EnitityVM
{
    public class MenuVM
    {
        public string ?Id_menu { get; set; }

        public string Title_menu { get; set; }

        public string ?Link_menu { get; set; }

        public string ?Content { get; set; }

        public bool ?Status { get; set; }

        public int Order_menu { get; set; }
    }
}

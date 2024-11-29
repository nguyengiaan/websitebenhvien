namespace websitebenhvien.Models.Enitity
{
    public class Menuchild
    {
        public string Id_MenuChild { get; set; }

        public string Title_MenuChild { get; set; }

        public string Link_MenuChild { get; set; }

        public string Id_menu { get; set; }

        public bool Status { get; set; }

        public int Order_menu { get; set; }

        public Menu Menu { get; set; }
    }
}

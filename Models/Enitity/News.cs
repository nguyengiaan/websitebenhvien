﻿namespace websitebenhvien.Models.Enitity
{
    public class News
    {
        public int Id_News { get; set; }
        public string Title { get; set; }

        public string? Description { get; set; }

        public string Url { get; set; }
        public string Alias_url { get; set; }

        public string Id_Categorynews { get; set; }

        public bool Status { get; set; }


        public DateTime Createat { get; set; }
        public Categorynews Categorynews { get; set; }



    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace websitebenhvien.Models.Enitity
{
    public class Notification
    {
        public string Id_Notification { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Createat { get; set; }

        public string Url { get; set; } 
        public bool Status { get; set; }
    }
}

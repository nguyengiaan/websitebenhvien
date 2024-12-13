
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace websitebenhvien.Models.Enitity
{
    public class Recruitment
    {
      
        public string Id_Recruitment { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string CV_Url { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Note { get; set; }
        public string Position { get; set; }    

        public string Sex { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace websitebenhvien.Models.Enitity
{

    public class ReceivingAngle
    {
        public int id { get; set; }

        public string title { get; set; }

        public string ? description { get; set; }

        public string ? image { get; set; }

        public string ? link { get; set; }
        public bool status { get; set; }
        public DateTime created_at { get; set; }

        public int? order { get; set; }


    }
}
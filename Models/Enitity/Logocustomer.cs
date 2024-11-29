using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace websitebenhvien.Models.Enitity
{
    public class Logocustomer
    {
        public string Id_logocustomer { get; set; }
        public string CustomerName { get; set; }
        public string Logo { get; set; }
        public DateTime Attime { get; set; }
        public bool Status { get; set; }    
    }
}

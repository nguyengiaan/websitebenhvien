using System;
using System.ComponentModel.DataAnnotations;

namespace websitebenhvien.Models.Enitity
{
    public class Chat
    {

        public string Id_chat { get; set; }
        public string Id_Sender { get; set; }
        public string Id_Receiver { get; set; }
        public string Message { get; set; }
        public DateTime Time { get; set; }
        public bool Status { get; set; }
    }
}

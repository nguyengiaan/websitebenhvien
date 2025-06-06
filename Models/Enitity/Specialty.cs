
using System;
using System.ComponentModel.DataAnnotations;
namespace websitebenhvien.Models.Enitity
{
    public class Specialty
    {

        public Specialty()
        {
            Postrelate = new List<Postrelate>();
            ListvideoSpectialty = new List<ListvideoSpectialty>();
            Doctor = new List<Doctor>();
            Makeanappointment = new List<Makeanappointment>();
        }
        
        public int Id_Specialty { get; set; }
        public string Title { get; set; }
        public string Introduction { get; set; }
        public string Machine { get; set; }
        public string Service { get; set; }

        public string Thumnail { get; set; }

        public string Alias_url { get; set; }

        public List<Postrelate> Postrelate { get; set; }

        public List<ListvideoSpectialty> ListvideoSpectialty { get; set; }

        public List<Doctor> Doctor { get; set; }

        public List<Makeanappointment> Makeanappointment { get; set; }



    }
}

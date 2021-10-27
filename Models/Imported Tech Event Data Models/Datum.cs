using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeatherProject.Models
{
    public class Datum
    {
        public int slug { get; set; }
        public string slugforurl { get; set; }
        public string summary { get; set; }
        public string summaryDisplay { get; set; }
        public string description { get; set; }
        public bool deleted { get; set; }
        public bool cancelled { get; set; }
        public bool is_physical { get; set; }
        public bool is_virtual { get; set; }
        public Custom_Fields custom_fields { get; set; }
        public string siteurl { get; set; }
        public string url { get; set; }
        public string ticket_url { get; set; }
        public string timezone { get; set; }
        public Start start { get; set; }
        public End end { get; set; }
        public Venue venue { get; set; }
        public Area[] areas { get; set; }
        public Country country { get; set; }
    }

}
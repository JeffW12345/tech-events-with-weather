using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeatherProject.Models
{
    public class Venue
    {
        public int slug { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string address { get; set; }
        public string addresscode { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeatherProject.Models
{
    public class Start
    {
        public int timestamp { get; set; }
        public string rfc2882utc { get; set; }
        public string rfc2882local { get; set; }
        public string displaylocal { get; set; }
        public string yearlocal { get; set; }
        public string monthlocal { get; set; }
        public string daylocal { get; set; }
        public string hourlocal { get; set; }
        public string minutelocal { get; set; }
        public string rfc2882timezone { get; set; }
        public string displaytimezone { get; set; }
        public string yeartimezone { get; set; }
        public string monthtimezone { get; set; }
        public string daytimezone { get; set; }
        public string hourtimezone { get; set; }
        public string minutetimezone { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeatherProject.Models
{
    public class Root
    {
        public Datum[] data { get; set; }
        public string localtimezone { get; set; }
    }
}
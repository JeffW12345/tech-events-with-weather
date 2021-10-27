using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
namespace WeatherProject.Models
{
    public class EventInfo
    {
        [Key]
        public int EventID { get; set; }
        public DateTime EventDate { get; set; }
        public string TimeOfEvent { get; set; }
        public string City { get; set; }
        public string Summary { get; set; }
        public string URL { get; set; }
        public string WeatherDescription { get; set; }
        public bool IsRemote { get; set; }
        public string Latitude { get; set; }
        public string Longtitude { get; set; }
    }

    

    }
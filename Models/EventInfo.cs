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
        [Display(Name = "Summary")]
        public string Summary { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Event date")]

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EventDate { get; set; }

        public int HourOfEvent { get; set; }
        public int MinOfEvent { get; set; }

        public string WeatherDescription { get; set; }

        public string SlugForURL { get; set; }

        public string URL { get; set; }
        public string Time { get; set; }

        public string City { get; set; }

        public bool IsRemote { get; set; }

        public string Latitude { get; set; }

        public string Longtitude { get; set; }

        public string TimeOfEvent { get; set; }
    }
    public class EventInfoContext : DbContext
    {
        public DbSet<EventInfo> EventInfo { get; set; }
    }
    public class Root
    {
        public Datum[] data { get; set; }
        public string localtimezone { get; set; }
    }

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

    public class Custom_Fields
    {
        public string code_of_conduct { get; set; }
    }

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

    public class End
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

    public class Country
    {
        public string title { get; set; }
    }

    public class Area
    {
        public int slug { get; set; }
        public string title { get; set; }
    }

    public class CreateObjects
    {
        internal static List<EventInfo> GetTechEvents()
        {
            List<EventInfo> techEvents = GetEventsWithoutWeather();
            AddWeather(techEvents);
            return techEvents;
        }

        private static void AddWeather(List<EventInfo> eventsWithoutWeather)
        {
            foreach(var techEvent in eventsWithoutWeather)
            {
                string url = "api.openweathermap.org/data/2.5/forecast?lat=" + techEvent.Latitude + "&lon=" + techEvent.Longtitude + "&appid=e713f513019bae6baa220783378d7945";
                string json = new System.Net.WebClient().DownloadString(url);
                var dataPoints = JsonConvert.DeserializeObject<Rootobject>(json);
                foreach(var snapshot in dataPoints.list)
                {
                    int year = Convert.ToInt32(snapshot.dt_txt.Substring(0, 4));
                    int month = Convert.ToInt32(snapshot.dt_txt.Substring(5, 2));
                    int day = Convert.ToInt32(snapshot.dt_txt.Substring(8, 2));
                    int hour = Convert.ToInt32(snapshot.dt_txt.Substring(11, 2));
                    int min = Convert.ToInt32(snapshot.dt_txt.Substring(14, 2));
                    DateTime timeOfWeather = new DateTime(year, month, day, hour, min, 0);
                    if(timeOfWeather < techEvent.EventDate)
                    {
                        continue;
                    }
                    else
                    {
                        techEvent.WeatherDescription = snapshot.weather[0].main + ": " + snapshot.weather[0].description;
                    }
                }
            }
        }

        private static List<EventInfo> GetEventsWithoutWeather()
        {
            string url = "https://opentechcalendar.co.uk/api1/events.json";
            string json = new System.Net.WebClient().DownloadString(url);
            var dataPoints = JsonConvert.DeserializeObject<Root>(json);
            List<EventInfo> eventsWithoutWeather = new List<EventInfo>();
            int eventNum = 0;
            foreach (var entry in dataPoints.data)
            {
                if (entry.cancelled || entry.country.title != "United Kingdom")
                {
                    continue;
                }
                EventInfo obj = new EventInfo();
                obj.EventDate = new DateTime(Convert.ToInt32(entry.start.yearlocal), Convert.ToInt32(entry.start.monthlocal), Convert.ToInt32(entry.start.daylocal),
                    Convert.ToInt32(entry.start.hourlocal), Convert.ToInt32(entry.start.minutelocal), 0);
                if (obj.EventDate > DateTime.Now.AddDays(5))
                {
                    continue;
                }
                obj.EventID = eventNum++;
                obj.Summary = entry.summary;
                var theDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
                obj.URL = "https://opentechcalendar.co.uk/event/" + entry.slugforurl;
                obj.City = entry.areas[0].title;
                obj.IsRemote = entry.is_virtual;
                obj.Latitude = entry.venue.lat;
                obj.Longtitude = entry.venue.lng;
                obj.TimeOfEvent = entry.start.hourlocal + ":" + entry.start.minutelocal;
                eventsWithoutWeather.Add(obj);
            }
            return eventsWithoutWeather;
        }
    }


    public class Rootobject
        {
            public string cod { get; set; }
            public float message { get; set; }
            public int cnt { get; set; }
            public List[] list { get; set; }
            public City city { get; set; }
        }

        public class City
        {
            public int id { get; set; }
            public string name { get; set; }
            public Coord coord { get; set; }
            public string country { get; set; }
        }

        public class Coord
        {
            public float lat { get; set; }
            public float lon { get; set; }
        }

        public class List
        {
            public int dt { get; set; }
            public Main main { get; set; }
            public Weather[] weather { get; set; }
            public Clouds clouds { get; set; }
            public Wind wind { get; set; }
            public Rain rain { get; set; }
            public Sys sys { get; set; }
            public string dt_txt { get; set; }
        }

        public class Main
        {
            public float temp { get; set; }
            public float temp_min { get; set; }
            public float temp_max { get; set; }
            public float pressure { get; set; }
            public float sea_level { get; set; }
            public float grnd_level { get; set; }
            public int humidity { get; set; }
            public int temp_kf { get; set; }
        }

        public class Clouds
        {
            public int all { get; set; }
        }

        public class Wind
        {
            public float speed { get; set; }
            public float deg { get; set; }
            public float gust { get; set; }
        }

        public class Rain
        {
        }

        public class Sys
        {
            public string pod { get; set; }
        }

        public class Weather
        {
            public int id { get; set; }
            public string main { get; set; }
            public string description { get; set; }
            public string icon { get; set; }
        }
    }